using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShareXe.Base.Dtos;
using ShareXe.Modules.Vehicles.Dtos;
using ShareXe.Modules.Vehicles.Services;

using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.Modules.Vehicles.Controllers
{
    [ApiController]
    [Route("/api/v1/vehicles")]
    [Produces("application/json")]
    public class VehicleController(VehiclesService vehiclesService) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            Summary = "Register a new vehicle",
            Description = "Allows an authenticated driver to register a new vehicle with an image."
        )]
        [SwaggerResponse(201, "Returns the newly created vehicle.")]
        [SwaggerResponse(400, "Bad Request - Invalid input data or file format.")]
        public async Task<ActionResult<SuccessResponse<VehicleDto>>> CreateVehicle([FromForm] CreateVehicleDto createVehicleDto)
        {
            var vehicle = await vehiclesService.CreateVehicleAsync(createVehicleDto);
            var vehicleDto = await vehiclesService.MapToVehicleDtoAsync(vehicle);

            // Trả về 201 Created cho chuẩn RESTful khi tạo mới resource
            return Created(string.Empty, SuccessResponse<VehicleDto>.WithData(vehicleDto, "Vehicle registered successfully."));
        }

        [Authorize]
        [HttpGet("me")]
        [SwaggerOperation(
            Summary = "Get my vehicles",
            Description = "Retrieves a list of all vehicles registered by the currently authenticated driver."
        )]
        [SwaggerResponse(200, "Returns the list of vehicles.")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<VehicleDto>>>> GetMyVehicles()
        {
            var vehicles = await vehiclesService.GetMyVehiclesAsync();
            var vehicleDtos = await Task.WhenAll(vehicles.Select(v => vehiclesService.MapToVehicleDtoAsync(v)));

            return Ok(SuccessResponse<IEnumerable<VehicleDto>>.WithData(vehicleDtos, "Vehicles retrieved successfully."));
        }
    }
}
