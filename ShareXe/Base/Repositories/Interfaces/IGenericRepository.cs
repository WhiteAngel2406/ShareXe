using ShareXe.Base.Entities;
using System.Linq.Expressions;

namespace ShareXe.Base.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Lấy tất cả
        Task<IReadOnlyList<T>> GetAllAsync();

        // Lấy theo ID
        Task<T?> GetByIdAsync(int id);

        // Thêm
        Task<T> AddAsync(T entity);

        // Sửa
        Task UpdateAsync(T entity);

        // Xóa
        Task DeleteAsync(T entity);

        // Nâng cao: Tìm kiếm theo điều kiện (Expression)
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
    }
}
