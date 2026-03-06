using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShareXe.Base.Dtos;
using ShareXe.src.Modules.Trips.Dtos;
using ShareXe.src.Modules.Trips.Services;

using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.src.Modules.Trips.Controller
{
    [ApiController]
    [Route("/api/v1/trips")]
    [Produces("application/json")]
    public class TripController(TripsService tripsService) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        [SwaggerOperation(
            Summary = "Create a new trip",
            Description = "Allows an authenticated driver to create a new ride-sharing trip."
        )]
        [SwaggerResponse(201, "Returns the newly created trip.")]
        [SwaggerResponse(400, "Bad Request - Invalid input data.")]
        public async Task<ActionResult<SuccessResponse<TripDto>>> CreateTrip([FromBody] CreateTripDto createTripDto)
        {
            var trip = await tripsService.CreateTripAsync(createTripDto);
            return Created(string.Empty, SuccessResponse<TripDto>.WithData(trip, "Trip created successfully."));
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Search available trips",
            Description = "Retrieves a list of available trips. (Later we can add filter options like StartHub, EndHub, Date...)"
        )]


        [SwaggerResponse(200, "Returns the list of trips.")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<TripDto>>>> GetTrips()
        {
            var trips = await tripsService.GetTripsAsync();
            return Ok(SuccessResponse<IEnumerable<TripDto>>.WithData(trips, "Trips retrieved successfully."));
        }
        [HttpGet("{id}/seat-map")]
        [SwaggerOperation(
            Summary = "Get seat map for a specific trip",
            Description = "Retrieves the full seating chart for a trip, showing exactly which seats are available, which are booked, and their coordinates."
        )]
        [SwaggerResponse(200, "Returns the real-time seat map.")]
        [SwaggerResponse(404, "Trip not found.")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<TripSeatDto>>>> GetTripSeatMap(Guid id)
        {
            var seatMap = await tripsService.GetTripSeatMapAsync(id);
            return Ok(SuccessResponse<IEnumerable<TripSeatDto>>.WithData(seatMap, "Seat map retrieved successfully."));
        }
    }
}
