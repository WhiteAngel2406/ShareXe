namespace ShareXe.Base.Helpers
{
    public static class DateTimeHelper
    {
        // 1. Lấy giờ Việt Nam hiện tại (GMT+7)
        public static DateTime GetVietnamTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        }

        // 2. Format thời gian tương đối (VD: "Vừa xong", "5 phút trước")
        public static string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = GetVietnamTime() - dateTime;

            if (timeSpan.TotalMinutes < 1) return "Vừa xong";
            if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes} phút trước";
            if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours} giờ trước";
            if (timeSpan.TotalDays < 7) return $"{(int)timeSpan.TotalDays} ngày trước";

            return dateTime.ToString("dd/MM/yyyy");
        }
    }
}
