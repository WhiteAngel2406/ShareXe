using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using ShareXe.Base.Entities;
using ShareXe.Models;

namespace ShareXe.Base.Repositories
{
    /// <summary>
    /// Generic base repository providing common CRUD operations and querying capabilities for entities.
    /// </summary>
    /// <typeparam name="T">The entity type that must inherit from <see cref="BaseEntity"/>.</typeparam>
    /// <remarks>
    /// This repository handles database operations including create, read, update, delete, and soft delete operations.
    /// It supports filtering, pagination, sorting, and entity tracking options through <see cref="QueryOptions{T}"/>.
    /// </remarks>
    public class BaseRepository<T>(ShareXeDbContext context) where T : BaseEntity
    {

        protected readonly DbSet<T> dbSet = context.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(T entity)
        {
            entity.DeletedAt = DateTime.Now;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            if (filter != null)
            {
                return await dbSet.CountAsync(filter);
            }
            return await dbSet.CountAsync();
        }

        public async Task<T?> GetByIdAsync(int id) => await dbSet.FindAsync(id);

        public async Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, string includeProperties = "")
        {
            IQueryable<T> query = dbSet.AsNoTracking();

            query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                ([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(QueryOptions<T> options)
        {
            var query = ApplyOptions(options);

            if (options.IsPagingEnabled)
            {
                query = query.Skip(options.Skip).Take(options.Take);
            }

            return await query.ToListAsync();
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetAllWithCountAsync(QueryOptions<T> options)
        {
            var query = ApplyOptions(options);

            var totalCount = await query.CountAsync();

            if (options.IsPagingEnabled)
            {
                query = query.Skip(options.Skip).Take(options.Take);
            }

            var items = await query.ToListAsync();

            return (items, totalCount);
        }

        private IQueryable<T> ApplyOptions(QueryOptions<T> options)
        {
            IQueryable<T> query = dbSet.AsNoTracking();

            if (options.Filter != null)
            {
                query = query.Where(options.Filter);
            }

            if (!options.IncludeDeleted)
            {
                query = query.Where(e => e.DeletedAt == null);
            }

            foreach (var includeProperty in options.IncludeProperties.Split
                ([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (options.OrderBy != null)
            {
                query = options.OrderBy(query);
            }

            return query;
        }

        public async Task UpdateAsync(T entity)
        {
            entity.LastModifiedAt = DateTimeOffset.UtcNow;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync() => await context.SaveChangesAsync();

        public IQueryable<T> Query() => dbSet.AsNoTracking();
    }

    public class QueryOptions<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Filter { get; set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; }
        public string IncludeProperties { get; set; } = string.Empty;

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool IsPagingEnabled { get; set; } = false;

        public bool IncludeDeleted { get; set; } = false;

        public int Skip => (PageIndex - 1) * PageSize;
        public int Take => PageSize;
    }
}
