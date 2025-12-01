using FinanceControl.Domain.Interfaces;
using FinanceControl.Infrastructure.Contexts;

namespace FinanceControl.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepository Categories { get; }
        public IPaymentMethodRepository PaymentMethods { get; }
        public ITransactionRepository Transactions { get; }
        public UnitOfWork(AppDbContext context,
            ICategoryRepository cateogories,
            IPaymentMethodRepository paymentMethods,
            ITransactionRepository transactions
            )
        {
            _context = context;
            Categories = cateogories;
            PaymentMethods = paymentMethods;
            Transactions = transactions;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
