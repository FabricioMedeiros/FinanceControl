using System.Linq.Expressions;

namespace FinanceControl.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity?>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task<int> SaveChangesAsync();
    }
}
