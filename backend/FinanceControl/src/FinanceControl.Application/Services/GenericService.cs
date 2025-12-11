using AutoMapper;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Utils;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
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
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor)
            : base(notificator, httpContextAccessor)
        {
            _uow = uow;
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<PagedResult<TDto>> GetAllAsync(
            Dictionary<string, string>? filters,
            int? pageNumber = null,
            int? pageSize = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            var currentUserId = GetCurrentUserId();
            var filterExpression = ApplyFilters(filters, currentUserId);

            int? skip = null;
            int? size = null;

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                skip = (pageNumber.Value - 1) * pageSize.Value;
                size = pageSize.Value;
            }

            var (items, totalRecords) = await _repository.GetAllAsync(
                filterExpression, includes, skip, size, currentUserId);

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
            var currentUserId = GetCurrentUserId();
            var entity = await _repository.GetByIdAsync(id, includes, currentUserId);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id, bool returnEntity, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            var currentUserId = GetCurrentUserId();
            return await _repository.GetByIdAsync(id, includes, currentUserId);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var currentUserId = GetCurrentUserId();
            return await _repository.ExistsAsync(predicate, currentUserId);
        }

        public virtual async Task<TDto?> AddAsync(TDto dto, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            var entity = _mapper.Map<TEntity>(dto);
            entity.UserId = GetCurrentUserId();

            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            var fullEntity = await _repository.GetByIdAsync(entity.Id, includes, entity.UserId);
            return _mapper.Map<TDto>(fullEntity);
        }

        public virtual async Task<TDto?> AddAsync(TEntity entity, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            entity.UserId = GetCurrentUserId();

            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            var fullEntity = await _repository.GetByIdAsync(entity.Id, includes, entity.UserId);
            return _mapper.Map<TDto>(fullEntity);
        }

        public virtual async Task UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            entity.UserId = GetCurrentUserId();

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            entity.UserId = GetCurrentUserId();

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var currentUserId = GetCurrentUserId();
            await _repository.DeleteAsync(id, currentUserId);

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