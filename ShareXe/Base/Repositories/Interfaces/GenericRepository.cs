using Microsoft.EntityFrameworkCore;
using ShareXe.Base.Entity;
using ShareXe.Base.Repositories.Implements;
using ShareXe.Models;
using System.Linq.Expressions;

namespace ShareXe.Base.Repositories.Interfaces
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ShareXeDbContext _context;

        public GenericRepository(ShareXeDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            // Tự động lọc các bản ghi chưa bị xóa (Soft Delete)
            return await _context.Set<T>()
                                 .Where(x => !x.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        // Hàm tìm kiếm linh hoạt
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                                .Where(x => !x.IsDeleted)
                                .Where(predicate)
                                .ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            // Cập nhật ngày sửa cuối cùng
            entity.LastModifiedAt = DateTime.Now;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
