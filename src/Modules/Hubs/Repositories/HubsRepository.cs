using Microsoft.EntityFrameworkCore;

using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.Hubs.Entities;

namespace ShareXe.Modules.Hubs.Repositories
{
    [Injectable]
    public class HubsRepository(ShareXeDbContext dbContext) : BaseRepository<Hub>(dbContext), IHubsRepository
    {
        public async Task<(IEnumerable<Hub> Items, int TotalCount)> SearchAsync(string? keyword, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            IQueryable<Hub> query = dbSet.AsNoTracking().Where(h => h.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string lowerKeyword = keyword.ToLower();
                query = query.Where(h => h.Name.ToLower().Contains(lowerKeyword) || h.Address.ToLower().Contains(lowerKeyword));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            List<Hub> items = await query
                .OrderByDescending(h => h.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}