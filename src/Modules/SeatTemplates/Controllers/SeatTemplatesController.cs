using Microsoft.AspNetCore.Mvc;

using ShareXe.Base.Dtos;
using ShareXe.src.Modules.SeatTemplates.Dtos;
using ShareXe.src.Modules.SeatTemplates.Services;

using Swashbuckle.AspNetCore.Annotations;

namespace ShareXe.src.Modules.SeatTemplates.Controllers
{
    [ApiController]
    [Route("/api/v1/seat-templates")]
    [Produces("application/json")]
    public class SeatTemplatesController(SeatTemplatesService seatTemplatesService) : ControllerBase
    {
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a seat template",
            Description = "Adds a new seat position to a specific vehicle type."
        )]
        [SwaggerResponse(201, "Returns the newly created seat template.")]
        public async Task<ActionResult<SuccessResponse<SeatTemplateDto>>> CreateSeatTemplate([FromBody] CreateSeatTemplateDto createDto)
        {
            var seatTemplate = await seatTemplatesService.CreateSeatTemplateAsync(createDto);
            return Created(string.Empty, SuccessResponse<SeatTemplateDto>.WithData(seatTemplate, "Seat template created successfully."));
        }

        [HttpGet("vehicle-type/{vehicleTypeId}")]
        [SwaggerOperation(
            Summary = "Get seat map by vehicle type",
            Description = "Retrieves the full seating layout for a specific vehicle type."
        )]
        [SwaggerResponse(200, "Returns the list of seat templates (the seating chart).")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<SeatTemplateDto>>>> GetTemplatesByVehicleType(Guid vehicleTypeId)
        {
            var templates = await seatTemplatesService.GetTemplatesByVehicleTypeIdAsync(vehicleTypeId);
            return Ok(SuccessResponse<IEnumerable<SeatTemplateDto>>.WithData(templates, "Seat map retrieved successfully."));
        }
    }
}
