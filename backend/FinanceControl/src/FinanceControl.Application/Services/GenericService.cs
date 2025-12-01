using AutoMapper;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Utils;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinanceControl.Application.Services
{
    public class GenericService<TEntity, TDto> : BaseService, IGenericService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : class
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GenericService(
            IUnitOfWork uow,
            IGenericRepository<TEntity> repository,
            IMapper mapper,
            INotificator notificator)
            : base(notificator)
        {
            _uow = uow;
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<PagedResult<TDto>> GetAllAsync(
            Dictionary<string, string>? filters,
            int? pageNumber = null,
            int? pageSize = null,
            Guid? userId = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            var filterExpression = ApplyFilters(filters, userId);

            int? skip = null;
            int? size = null;

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                skip = (pageNumber.Value - 1) * pageSize.Value;
                size = pageSize.Value;
            }

            var (items, totalRecords) = await _repository.GetAllAsync(filterExpression, includes, skip, size);

            return new PagedResult<TDto>
            {
                Page = pageNumber ?? 1,
                PageSize = pageSize ?? totalRecords,
                TotalRecords = totalRecords,
                Items = _mapper.Map<IEnumerable<TDto>>(items)
            };
        }

        public virtual async Task<TDto?> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            var entity = await _repository.GetByIdAsync(id, includes);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id, bool returnEntity, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            return await _repository.GetByIdAsync(id, includes);
        }

        public virtual async Task<TDto?> AddAsync(TDto dto, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            var fullEntity = await _repository.GetByIdAsync(entity.Id, includes);
            return _mapper.Map<TDto>(fullEntity);
        }

        public virtual async Task<TDto?> AddAsync(TEntity entity, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            var fullEntity = await _repository.GetByIdAsync(entity.Id, includes);
            return _mapper.Map<TDto>(fullEntity);
        }

        public virtual async Task UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);

            try
            {
                await _uow.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;

                if (message.Contains("REFERENCE constraint"))
                    Notify("Não é possível excluir, existem registros vinculados.");
                else
                    Notify("Erro inesperado ao processar a operação.");
            }
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