using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using FinanceControl.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
namespace FinanceControl.Infrastructure.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context) : base(context) { }

        public async Task<List<Transaction>> GetTransactionsByPeriodAsync(
            DateTime startDate,
            DateTime endDate,
            Guid? categoryId = null,
            Guid? paymentMethodId = null)
        {
            var query = _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.PaymentMethod)
                .AsQueryable();

            query = query.Where(t => t.Date >= startDate && t.Date <= endDate);

            if (categoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            if (paymentMethodId.HasValue)
            {
                query = query.Where(t => t.PaymentMethodId == paymentMethodId.Value);
            }

            return await query.ToListAsync();
        }
    }
}