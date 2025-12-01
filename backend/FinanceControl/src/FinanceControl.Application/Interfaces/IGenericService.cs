using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Interfaces
{
    public interface IGenericService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : class
    {
        Task<PagedResult<TDto>> GetAllAsync(
            Dictionary<string, string>? filters,
            int? pageNumber = null,
            int? pageSize = null,
            Guid? userId = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<TDto?> GetByIdAsync(
            Guid id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<TEntity?> GetByIdAsync(
            Guid id,
            bool returnEntity,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<TDto?> AddAsync(
            TDto dto,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<TDto?> AddAsync(
            TEntity entity,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task UpdateAsync(TDto dto);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(Guid id);
    }
}