using Microsoft.EntityFrameworkCore;
using ShareXe.Base.Attributes;

namespace ShareXe.Models
{
    [Injectable]
    public class ShareXeDbContext(DbContextOptions<ShareXeDbContext> options) : DbContext(options)
    {

        // Khai báo các bảng ở đây. Ví dụ:
        // public DbSet<Car> Cars { get; set; }
        // public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
