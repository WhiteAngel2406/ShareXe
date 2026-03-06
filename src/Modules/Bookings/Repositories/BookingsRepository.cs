using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.Bookings.Entities;

namespace ShareXe.src.Modules.Bookings.Repositories
{
    [Injectable]
    public class BookingsRepository(ShareXeDbContext context) : BaseRepository<Booking>(context)
    {
    }
}
