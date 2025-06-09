using FinanceControl.Domain.Entities;

namespace FinanceControl.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionsAsync(int year, int? month = null);
    }
}
