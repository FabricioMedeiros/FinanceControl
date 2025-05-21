using FinanceControl.Domain.Entities;

namespace FinanceControl.Domain.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<List<Transaction>> GetTransactionsByPeriodAsync(
            DateTime startDate,
            DateTime endDate,
            Guid? categoryId = null,
            Guid? paymentMethodId = null);
    }
}
