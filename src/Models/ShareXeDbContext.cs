using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShareXe.Base.Attributes;

namespace ShareXe.Models
{
    [Injectable]
    public class ShareXeDbContext(DbContextOptions<ShareXeDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Quét các class có gắn [EntityAttribute]
            var entityTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<EntityAttribute>() != null && !t.IsAbstract);

            foreach (var type in entityTypes)
            {
                var entityBuilder = modelBuilder.Entity(type);
                var attr = type.GetCustomAttribute<EntityAttribute>();

                // Cấu hình tên bảng
                if (!string.IsNullOrEmpty(attr?.TableName))
                {
                    entityBuilder.ToTable(attr.TableName);
                }

                // 2. Xử lý Unique và Composite Unique
                // Gom nhóm các thuộc tính dựa trên GroupName của UniqueAttribute
                var uniqueGroups = type.GetProperties()
                    .Select(p => new
                    {
                        Property = p,
                        UniqueAttr = p.GetCustomAttribute<UniqueAttribute>()
                    })
                    .Where(x => x.UniqueAttr != null)
                    .GroupBy(x => x.UniqueAttr!.GroupName ?? x.Property.Name);
                // Nếu không có GroupName, dùng tên Property để tạo Single Unique Index

                foreach (var group in uniqueGroups)
                {
                    var propNames = group.Select(x => x.Property.Name).ToArray();

                    // Tạo Index cho nhóm các cột này và đánh dấu là Unique
                    entityBuilder.HasIndex(propNames).IsUnique();
                }
            }

            // 3. Cấu hình UTC cho DateTimeOffset
            // Converter này đảm bảo khi lưu xuống DB sẽ gọi ToUniversalTime()
            var utcConverter = new ValueConverter<DateTimeOffset, DateTimeOffset>(
                v => v.ToUniversalTime(),
                v => v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    // Quét cả kiểu DateTimeOffset và DateTimeOffset? (nullable)
                    if (property.ClrType == typeof(DateTimeOffset) || property.ClrType == typeof(DateTimeOffset?))
                    {
                        property.SetValueConverter(utcConverter);
                    }
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
