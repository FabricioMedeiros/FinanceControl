using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using FinanceControl.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        protected readonly AppDbContext _context;
        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(int year, int? month = null)
        {
            var query = _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.PaymentMethod)
                .AsQueryable();

            query = query.Where(t => t.Date.Year == year);

            if (month.HasValue)
                query = query.Where(t => t.Date.Month == month.Value);

            return await query.ToListAsync();
        }

    }
}
