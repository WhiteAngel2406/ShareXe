using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.Trips.Entities;

namespace ShareXe.Modules.Trips.Repositories
{
    [Injectable]
    public class TripsRepository(ShareXeDbContext context) : BaseRepository<Trip>(context)
    {
    }
}
