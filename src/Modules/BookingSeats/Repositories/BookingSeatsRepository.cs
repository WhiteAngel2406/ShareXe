using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.BookingSeats.Entities;

namespace ShareXe.Modules.BookingSeats.Repositories
{
    [Injectable]
    public class BookingSeatsRepository(ShareXeDbContext context) : BaseRepository<BookingSeat>(context)
    {
    }
}
