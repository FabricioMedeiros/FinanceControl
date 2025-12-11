using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using FinanceControl.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinanceControl.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
    
        protected IQueryable<TEntity> ApplyIncludes(
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes,
            Guid? userId = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
                query = includes(query);

            if (userId.HasValue && typeof(TEntity).GetProperty("UserId") != null)
                query = query.Where(e => e.UserId == userId.Value);

            return query;
        }

        public async Task<(IEnumerable<TEntity> Items, int TotalRecords)> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            int? skip = null,
            int? take = null,
            Guid? userId = null)
        {
            IQueryable<TEntity> query = ApplyIncludes(includes, userId);

            if (filter != null)
                query = query.Where(filter);

            int totalRecords = await query.CountAsync();

            if (skip.HasValue) query = query.Skip(skip.Value);
            if (take.HasValue) query = query.Take(take.Value);

            var items = await query.ToListAsync();
            return (items, totalRecords);
        }

        public async Task<TEntity?> GetByIdAsync(
            Guid id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            Guid? userId = null)
        {
            IQueryable<TEntity> query = ApplyIncludes(includes, userId);
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, Guid? userId = null)
        {
            IQueryable<TEntity> query = ApplyIncludes(null, userId);
            var entity = await query.FirstOrDefaultAsync(e => e.Id == id);

            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, Guid? userId = null)
        {
            IQueryable<TEntity> query = ApplyIncludes(null, userId);
            return await query.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, Guid? userId = null)
        {
            IQueryable<TEntity> query = ApplyIncludes(null, userId);

            if (predicate != null)
                query = query.Where(predicate);

            return await query.CountAsync();
        }
    }
}