using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.SeatTemplates.Entities;

namespace ShareXe.Modules.SeatTemplates.Repositories
{
    [Injectable]
    public class SeatTemplatesRepository(ShareXeDbContext context) : BaseRepository<SeatTemplate>(context)
    {
    }
}
