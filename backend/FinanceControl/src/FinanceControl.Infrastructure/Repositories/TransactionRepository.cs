using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using FinanceControl.Infrastructure.Contexts;

namespace FinanceControl.Infrastructure.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context) : base(context)
        {
        }
    }
}