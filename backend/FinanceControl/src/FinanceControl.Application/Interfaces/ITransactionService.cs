using FinanceControl.Application.DTOs;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Interfaces
{
    public interface ITransactionService : IGenericService<Transaction, TransactionDto>
    {
        Task<PagedResult<TransactionDto>> GetTransactionsByPeriodAsync(
                          DateTime startDate,
                          DateTime endDate,
                          Guid? categoryId = null,
                          Guid? paymentMethodId = null,
                          int? pageNumber = null,
                          int? pageSize = null);
    }
}
