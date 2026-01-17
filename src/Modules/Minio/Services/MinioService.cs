using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;

using ShareXe.Base.Attributes;
using ShareXe.Modules.Minio.Dtos;

namespace ShareXe.Modules.Minio.Services
{
    [Injectable]
    public partial class MinioService(IMinioClient minioClient, IConfiguration config, ILogger<MinioService> logger)
    {
        private readonly string _bucket = config["MINIO_BUCKET"] ?? "sharexe_bucket";
        private readonly string _publicEndpoint = config["MINIO_PUBLIC_ENDPOINT"] ?? "";

        public async Task<MinioFileResponse> UploadFileAsync(IFormFile file, string? folder)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var uniqueId = Guid.NewGuid().ToString();
            var originalFileName = SanitizeFileName(file.FileName);

            string savedFileName = string.IsNullOrWhiteSpace(folder)
                ? $"{timestamp}-{uniqueId}-{originalFileName}"
                : $"{folder}/{timestamp}-{uniqueId}-{originalFileName}";

            using var stream = file.OpenReadStream();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucket)
                .WithObject(savedFileName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType);

            await minioClient.PutObjectAsync(putObjectArgs);

            return new MinioFileResponse
            {
                FileName = originalFileName,
                Url = await GeneratePresignedUrlAsync(savedFileName)
            };
        }

        public async Task<string?> GeneratePresignedUrlAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            var args = new PresignedGetObjectArgs()
                .WithBucket(_bucket)
                .WithObject(fileName)
                .WithExpiry(60 * 60); // 1 hour

            string url = await minioClient.PresignedGetObjectAsync(args);

            var result = Regex.Replace(url, $"http.*{_bucket}", _publicEndpoint);
            return Regex.Replace(result, @"\?.+", "");
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var args = new RemoveObjectArgs()
                .WithBucket(_bucket)
                .WithObject(fileName);

            await minioClient.RemoveObjectAsync(args);
        }

        public async Task<bool> CheckFileExistsAsync(string fileName)
        {
            try
            {
                var args = new StatObjectArgs()
                    .WithBucket(_bucket)
                    .WithObject(fileName);

                await minioClient.StatObjectAsync(args);
                return true;
            }
            catch (ObjectNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking if file exists: {FileName}", fileName);
                throw;
            }
        }

        private static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return "untitled";

            string normalized = fileName.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new();

            foreach (char c in normalized)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            var sanitized = sb.ToString().Normalize(NormalizationForm.FormC);

            sanitized = WhitespaceRegex().Replace(sanitized, "_");
            sanitized = NonAlphanumericRegex().Replace(sanitized, "");

            return string.IsNullOrEmpty(sanitized) ? "untitled" : sanitized;
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhitespaceRegex();

        [GeneratedRegex(@"[^a-zA-Z0-9_\-\.]+")]
        private static partial Regex NonAlphanumericRegex();

        public async Task<Collection<Bucket>> GetBucketsAsync()
        {
            var result = await minioClient.ListBucketsAsync();
            return result.Buckets;
        }
    }
}
