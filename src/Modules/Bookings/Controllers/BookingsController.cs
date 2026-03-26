using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShareXe.Base.Dtos;
using ShareXe.Modules.Bookings.Dtos;
using ShareXe.Modules.Bookings.Services;

using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.Modules.Bookings.Controllers
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

        [HttpPost("{id}/cancel")]
        [SwaggerOperation(
            Summary = "Cancel a booking and refund",
            Description = "Cancels a specific booking. If the booking was already paid (Confirmed), the system will automatically refund the full amount to the user's wallet."
        )]
        [SwaggerResponse(200, "Returns the cancelled booking details.")]
        [SwaggerResponse(400, "Cannot cancel this booking.")]
        public async Task<ActionResult<SuccessResponse<BookingDto>>> CancelBooking(Guid id)
        {
            var booking = await bookingsService.CancelBookingAsync(id);
            return Ok(SuccessResponse<BookingDto>.WithData(booking, "Hủy chuyến thành công. Số ghế đã được giải phóng."));
        }
    }
}
