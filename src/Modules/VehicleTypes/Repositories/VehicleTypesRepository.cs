using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.VehicleTypes.Entities;

namespace ShareXe.Modules.VehicleTypes.Repositories
{
    [Injectable]
    public class VehicleTypesRepository(ShareXeDbContext context) : BaseRepository<VehicleType>(context)
    {
    }
}
