using FinanceControl.Domain.Entities;

namespace FinanceControl.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionsByPeriodAsync(Guid userId, int year, int? month = null);
        Task<IEnumerable<Transaction>> GetTransactionsForPaymentMethodBalanceAsync(Guid userId);
    }
}
