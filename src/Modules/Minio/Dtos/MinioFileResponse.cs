namespace ShareXe.Modules.Minio.Dtos
{
  public class MinioFileResponse
  {
    public string FileName { get; set; } = string.Empty;
    public string? Url { get; set; } = string.Empty;
  }
}