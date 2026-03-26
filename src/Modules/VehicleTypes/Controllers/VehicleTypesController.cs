using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShareXe.Base.Dtos;
using ShareXe.Modules.VehicleTypes.Dtos;
using ShareXe.Modules.VehicleTypes.Services;

using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.Modules.VehicleTypes.Controllers
{
    [ApiController]
    [Route("/api/v1/vehicle-types")]
    [Produces("application/json")]
    public class VehicleTypesController(VehicleTypesService vehicleTypesService) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all vehicle types",
            Description = "Retrieves a list of all available vehicle types (e.g., 4 seats, 7 seats, motorbikes)."
        )]
        [SwaggerResponse(200, "Returns the list of vehicle types.")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<VehicleTypeDto>>>> GetAllVehicleTypes()
        {
            var vehicleTypes = await vehicleTypesService.GetAllVehicleTypesAsync();
            return Ok(SuccessResponse<IEnumerable<VehicleTypeDto>>.WithData(vehicleTypes, "Vehicle types retrieved successfully."));
        }

        [HttpPost]
        [Authorize] // Thường API này sẽ được cấp quyền riêng cho Admin (VD: [Authorize(Roles = "Admin")])
        [SwaggerOperation(
            Summary = "Create a new vehicle type",
            Description = "Creates a new vehicle type category."
        )]
        [SwaggerResponse(201, "Returns the newly created vehicle type.")]
        public async Task<ActionResult<SuccessResponse<VehicleTypeDto>>> CreateVehicleType([FromBody] CreateVehicleTypeDto createDto)
        {
            var vehicleType = await vehicleTypesService.CreateVehicleTypeAsync(createDto);
            return Created(string.Empty, SuccessResponse<VehicleTypeDto>.WithData(vehicleType, "Vehicle type created successfully."));
        }
    }
}
