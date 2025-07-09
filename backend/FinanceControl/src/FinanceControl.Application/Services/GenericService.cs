using AutoMapper;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Utils;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using System.Linq.Expressions;

namespace FinanceControl.Application.Services
{
    public class GenericService<TEntity, TDto> : BaseService, IGenericService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GenericService(
            IGenericRepository<TEntity> repository,
            IMapper mapper,
            INotificator notificator)
            : base(notificator)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<PagedResult<TDto>> GetAllAsync(
            Dictionary<string, string>? filters,
            int? pageNumber = null,
            int? pageSize = null,
            Guid? userId = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var filterExpression = ApplyFilters(filters, userId);

            int page = pageNumber ?? 1;
            int size = pageSize ?? 10;
            int skip = (page - 1) * size;

            var (items, totalRecords) = await _repository.GetAllAsync(filterExpression, skip, size, includes);

            return new PagedResult<TDto>
            {
                Page = page,
                PageSize = size,
                TotalRecords = totalRecords,
                Items = _mapper.Map<IEnumerable<TDto>>(items)
            };
        }

        public virtual async Task<TDto?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes)
        {
            var entity = await _repository.GetByIdAsync(id, includes);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id, bool returnEntity, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _repository.GetByIdAsync(id, includeProperties);
        }

        public virtual async Task<TDto?> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<TDto>(createdEntity);
        }

        public virtual async Task<TDto?> AddAsync(TEntity entity)
        {
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<TDto>(createdEntity);
        }

        public virtual async Task UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.UpdateAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        private static Expression<Func<TEntity, bool>> ApplyFilters(Dictionary<string, string>? filters, Guid? userId)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            Expression combinedExpression = Expression.Constant(true);

            if (userId.HasValue && typeof(TEntity).GetProperty("UserId") != null)
            {
                var userIdComparison = CreateUserIdFilterExpression(parameter, userId.Value);
                combinedExpression = Expression.AndAlso(combinedExpression, userIdComparison);
            }

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    try
                    {
                        var comparison = ExpressionHelper.CreateNestedComparisonExpression(parameter, filter.Key, filter.Value);
                        combinedExpression = Expression.AndAlso(combinedExpression, comparison);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return Expression.Lambda<Func<TEntity, bool>>(combinedExpression, parameter);
        }

        private static Expression CreateUserIdFilterExpression(ParameterExpression parameter, Guid userId)
        {
            var property = Expression.Property(parameter, "UserId");
            var constant = Expression.Constant(userId);
            return Expression.Equal(property, constant);
        }
    }
}
