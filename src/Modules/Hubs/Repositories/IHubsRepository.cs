using ShareXe.Base.Repositories;
using ShareXe.Modules.Hubs.Entities;

namespace ShareXe.Modules.Hubs.Repositories
{
    public interface IHubsRepository
    {
        Task<(IEnumerable<Hub> Items, int TotalCount)> SearchAsync(string? keyword, int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}