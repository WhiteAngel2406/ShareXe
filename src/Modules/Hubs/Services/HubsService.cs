using AutoMapper;

using ShareXe.Base.Attributes;
using ShareXe.Base.Dtos;
using ShareXe.Modules.Hubs.Dtos;
using ShareXe.Modules.Hubs.Entities;
using ShareXe.Modules.Hubs.Repositories;

namespace ShareXe.Modules.Hubs.Services
{
    [Injectable]
    public class HubsService(IHubsRepository hubsRepository, IMapper mapper) : IHubsService
    {
        public async Task<PagedResponse<HubDto>> SearchHubsAsync(GetHubsRequest request, CancellationToken cancellationToken)
        {
            int page = request.Page ?? 1;
            int pageSize = request.PageSize ?? 10;

            (IEnumerable<Hub> items, int totalCount) = await hubsRepository.SearchAsync(
                request.Keyword, 
                page, 
                pageSize, 
                cancellationToken);

            List<HubDto> dtoList = mapper.Map<List<HubDto>>(items);

            return PagedResponse<HubDto>.WithPaging(dtoList, totalCount, page, pageSize);
        }

        public async Task<IEnumerable<HubDto>> GetNearbyHubsAsync(GetNearbyHubsRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<Hub> hubs = await hubsRepository.GetNearbyAsync(
                request.Latitude, 
                request.Longitude, 
                20000, // 20km
                100, // limit to 100
                cancellationToken);

            return mapper.Map<IEnumerable<HubDto>>(hubs);
        }
    }
}