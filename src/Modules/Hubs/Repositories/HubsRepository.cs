using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

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

        public async Task<IEnumerable<Hub>> GetNearbyAsync(double latitude, double longitude, double maxDistanceMeters, int count, CancellationToken cancellationToken)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            // Longitude first for NTS Coordinate
            var location = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));

            return await dbSet
                .AsNoTracking()
                .Where(h => h.DeletedAt == null && h.IsActive && h.Location.Distance(location) <= maxDistanceMeters)
                .OrderBy(h => h.Location.Distance(location))
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}