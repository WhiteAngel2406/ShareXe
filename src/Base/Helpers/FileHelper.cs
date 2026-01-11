namespace ShareXe.Base.Helpers
{
    public static class FileHelper
    {
        
        public static async Task<string> SaveImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0) return null;
            // 1. Tạo tên file độc nhất (dùng Guid để tránh trùng tên)
            var extension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            // 2. Xác định đường dẫn lưu vật lý trên ổ cứng
            // Đường dẫn sẽ là: [Thư mục Project]/wwwroot/uploads/[folderName]
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);

            // Nếu thư mục chưa có thì tạo mới
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var filePath = Path.Combine(path, uniqueFileName);

            // 3. Thực hiện lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 4. Trả về đường dẫn tương đối (để Frontend hiển thị được)
            return $"/uploads/{folderName}/{uniqueFileName}";
        }
    }
}
