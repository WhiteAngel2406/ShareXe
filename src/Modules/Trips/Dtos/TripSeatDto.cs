namespace ShareXe.src.Modules.Trips.Dtos
{
    public class TripSeatDto
    {
        public string SeatCode { get; set; } = null!;
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        // Ghế này là ghế tài xế? (Để Front-end ẩn đi hoặc bôi xám đen)
        public bool IsDriverSeat { get; set; }

        // CỰC KỲ QUAN TRỌNG: Ghế này còn trống không?
        public bool IsAvailable { get; set; }

        // Giá tiền của ghế (Lấy từ giá của chuyến đi)
        public decimal Price { get; set; }
    }
}
