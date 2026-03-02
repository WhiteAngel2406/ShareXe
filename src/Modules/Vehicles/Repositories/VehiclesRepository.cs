using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.Vehicles.Entities;

namespace ShareXe.src.Modules.Vehicles.Repositories
{
    [Injectable]
    public class VehiclesRepository(ShareXeDbContext context) : BaseRepository<Vehicle>(context)
    {
    }
}
