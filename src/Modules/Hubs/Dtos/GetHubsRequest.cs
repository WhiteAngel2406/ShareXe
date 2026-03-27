using ShareXe.Base.Dtos;

namespace ShareXe.Modules.Hubs.Dtos
{
    public class GetHubsRequest : PagedRequest
    {
        public string? Keyword { get; set; }
    }
}