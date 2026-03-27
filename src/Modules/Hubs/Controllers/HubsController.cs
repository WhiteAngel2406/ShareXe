using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using ShareXe.Base.Dtos;
using ShareXe.Modules.Hubs.Dtos;
using ShareXe.Modules.Hubs.Services;

namespace ShareXe.Modules.Hubs.Controllers
{
    [ApiController]
    [Route("api/v1/hubs")]
    public class HubsController(IHubsService hubsService) : ControllerBase
    {
        [HttpGet("search")]
        [SwaggerOperation(
            Summary = "Search for hubs",
            Description = "Retrieves a paginated list of hubs matching the search keyword. The keyword performs a wildcard search on the hub name or address."
        )]
        [SwaggerResponse(200, "Returns the matched hubs in a paginated format.", typeof(SuccessResponse<PagedResponse<HubDto>>))]
        [SwaggerResponse(400, "Bad Request - Invalid pagination parameters.")]
        public async Task<ActionResult<SuccessResponse<PagedResponse<HubDto>>>> SearchHubs([FromQuery] GetHubsRequest request, CancellationToken cancellationToken)
        {
            PagedResponse<HubDto> pagedResponse = await hubsService.SearchHubsAsync(request, cancellationToken);
            return Ok(SuccessResponse<PagedResponse<HubDto>>.WithData(pagedResponse, "Hubs retrieved successfully."));
        }
    }
}