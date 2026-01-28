namespace ShareXe.src.Base.Enums
{
    public enum BookingStatus
    {
        Pending = 0,    // Đã chọn ghế, chờ thanh toán
        Confirmed = 1,  // Đã thanh toán/giữ chỗ thành công
        Cancelled = 2,  // Khách hủy hoặc quá hạn thanh toán
        Completed = 3   // Đã đi xong
    }
}
