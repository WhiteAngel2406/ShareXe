using AutoMapper;

using ShareXe.Modules.Bookings.Entities;
using ShareXe.src.Modules.Bookings.Dtos;

namespace ShareXe.src.Modules.Bookings.Mappers
{
    public class BookingsProfile : Profile
    {
        public BookingsProfile()
        {
            // Ánh xạ từ Request (CreateBookingDto) sang Entity (Booking)
            // Bỏ qua BookingSeats vì ta sẽ map thủ công từ danh sách SeatIds trong Service
            CreateMap<CreateBookingDto, Booking>()
                .ForMember(dest => dest.BookingSeats, opt => opt.Ignore());

            // Ánh xạ từ Entity (Booking) sang Response (BookingDto) để trả về cho Client
            CreateMap<Booking, BookingDto>();
        }
    }
}
