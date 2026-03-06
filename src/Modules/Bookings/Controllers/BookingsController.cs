using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShareXe.Base.Dtos;
using ShareXe.src.Modules.Bookings.Dtos;
using ShareXe.src.Modules.Bookings.Services;

using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.src.Modules.Bookings.Controllers
{
    [ApiController]
    [Route("/api/v1/bookings")]
    [Produces("application/json")]
    [Authorize] // Bắt buộc khách hàng phải đăng nhập mới được đặt xe
    public class BookingsController(BookingsService bookingsService) : ControllerBase
    {
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new booking",
            Description = "Allows an authenticated user (passenger) to book a trip."
        )]
        [SwaggerResponse(201, "Returns the newly created booking.")]
        [SwaggerResponse(400, "Bad Request - Invalid trip or seats not available.")]
        public async Task<ActionResult<SuccessResponse<BookingDto>>> CreateBooking([FromBody] CreateBookingDto createDto)
        {
            var booking = await bookingsService.CreateBookingAsync(createDto);
            return Created(string.Empty, SuccessResponse<BookingDto>.WithData(booking, "Booking created successfully."));
        }

        [HttpGet("my-bookings")]
        [SwaggerOperation(
            Summary = "Get my bookings",
            Description = "Retrieves a list of bookings made by the current authenticated user."
        )]
        [SwaggerResponse(200, "Returns the list of the user's bookings.")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<BookingDto>>>> GetMyBookings()
        {
            var bookings = await bookingsService.GetMyBookingsAsync();
            return Ok(SuccessResponse<IEnumerable<BookingDto>>.WithData(bookings, "Bookings retrieved successfully."));
        }

        [HttpPost("{id}/pay")]
        [SwaggerOperation(
            Summary = "Pay for a booking",
            Description = "Deducts the total price of the booking from the user's wallet. The booking must be in Pending status."
        )]
        [SwaggerResponse(200, "Returns the confirmed booking.")]
        [SwaggerResponse(400, "Insufficient balance or invalid booking status.")]
        public async Task<ActionResult<SuccessResponse<BookingDto>>> PayBooking(Guid id)
        {
            var booking = await bookingsService.PayBookingAsync(id);
            return Ok(SuccessResponse<BookingDto>.WithData(booking, "Thanh toán chuyến đi thành công."));
        }
    }
}
