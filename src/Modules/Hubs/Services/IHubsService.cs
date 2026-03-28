using ShareXe.Base.Dtos;
using ShareXe.Modules.Hubs.Dtos;

namespace ShareXe.Modules.Hubs.Services
{
    public interface IHubsService
    {
        Task<PagedResponse<HubDto>> SearchHubsAsync(GetHubsRequest request, CancellationToken cancellationToken);

        Task<IEnumerable<HubDto>> GetNearbyHubsAsync(GetNearbyHubsRequest request, CancellationToken cancellationToken);
    }
}