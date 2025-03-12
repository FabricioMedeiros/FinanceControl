using AutoMapper;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

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

        public virtual async Task<PagedResult<TDto>> GetAllAsync(Dictionary<string, string>? filters, int? pageNumber = null, int? pageSize = null)
        {
            var query = _repository.Query();
            query = ApplyFilters(query, filters);
            return await ApplyPagination(query, pageNumber, pageSize);
        }

        public virtual async Task<TDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id, bool returnEntity)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<TDto?> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<TDto>(dto);
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

        private IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> query, Dictionary<string, string>? filters)
        {
            if (filters == null) return query;

            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            Expression combinedExpression = Expression.Constant(true);

            foreach (var filter in filters)
            {
                try
                {
                    var comparison = CreateNestedComparisonExpression(parameter, filter.Key, filter.Value);
                    combinedExpression = Expression.AndAlso(combinedExpression, comparison);
                }
                catch { continue; }
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(combinedExpression, parameter);
            return query.Where(lambda);
        }

        private async Task<PagedResult<TDto>> ApplyPagination(IQueryable<TEntity> query, int? pageNumber, int? pageSize)
        {
            int totalRecords = query.Count();
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            var items = _mapper.Map<IEnumerable<TDto>>(await Task.FromResult(query.ToList()));

            return new PagedResult<TDto>
            {
                Page = pageNumber ?? 1,
                PageSize = pageSize ?? totalRecords,
                TotalRecords = totalRecords,
                Items = items
            };
        }

        private static Expression CreateNestedComparisonExpression(ParameterExpression parameter, string propertyPath, string filterValue)
        {
            var properties = propertyPath.Split('.');
            Expression expression = parameter;

            foreach (var prop in properties)
            {
                var propertyInfo = expression.Type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                    throw new InvalidOperationException($"Property '{prop}' not found on type '{expression.Type.Name}'");

                expression = Expression.Property(expression, propertyInfo);
            }
            return CreateComparisonExpression(expression, ((MemberExpression)expression).Type, filterValue);
        }

        private static Expression CreateComparisonExpression(Expression left, Type propertyType, string filterValue)
        {
            object parsedValue = Convert.ChangeType(filterValue, Nullable.GetUnderlyingType(propertyType) ?? propertyType);
            Expression right = Expression.Constant(parsedValue, left.Type);
            return Expression.Equal(left, right);
        }
    }
}
