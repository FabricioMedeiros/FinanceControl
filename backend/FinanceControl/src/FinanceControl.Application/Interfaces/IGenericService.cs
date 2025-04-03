using FinanceControl.Domain.Entities;
using System.Linq.Expressions;

namespace FinanceControl.Application.Interfaces
{
    public interface IGenericService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : class
    {
        Task<PagedResult<TDto>> GetAllAsync(Dictionary<string, string>? filters, int? pageNumber = null, int? pageSize = null, Guid? userId = null, params Expression<Func<TEntity, object>>[] includes);
        Task<TDto?> GetByIdAsync(Guid id);
        Task<TEntity?> GetByIdAsync(Guid id, bool returnEntity);
        Task<TDto?> AddAsync(TDto dto);
        Task<TDto?> AddAsync(TEntity entity);
        Task UpdateAsync(TDto dto);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
    }
}