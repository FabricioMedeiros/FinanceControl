using System.Linq.Expressions;

namespace FinanceControl.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<(IEnumerable<TEntity> Items, int TotalRecords)> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            int? skip = null,
            int? take = null);

        Task<TEntity?> GetByIdAsync(
            Guid id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

        Task<TEntity> AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(Guid id);

        Task<int> SaveChangesAsync();
    }
}