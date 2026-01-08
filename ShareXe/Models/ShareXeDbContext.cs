using Microsoft.EntityFrameworkCore;

namespace ShareXe.Models
{
    public class ShareXeDbContext : DbContext
    {
        public ShareXeDbContext(DbContextOptions<ShareXeDbContext> options) : base(options)
        {
        }

        // Khai báo các bảng ở đây. Ví dụ:
        // public DbSet<Car> Cars { get; set; }
        // public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
