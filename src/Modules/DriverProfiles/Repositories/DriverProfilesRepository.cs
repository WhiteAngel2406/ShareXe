using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.DriverProfiles.Entities;

namespace ShareXe.src.Modules.DriverProfiles.Repositories
{
    [Injectable]
    public class DriverProfilesRepository(ShareXeDbContext context) : BaseRepository<DriverProfile>(context)
    {
    }
}
