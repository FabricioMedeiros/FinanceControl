using FinanceControl.Application.DTOs;
using FinanceControl.Domain.Entities;
using MediatR;

namespace FinanceControl.Application.Features.Transactions.Queries
{
    public sealed record GetAllTransactionsQuery(
        Dictionary<string, string>? Filters,
        int? PageNumber,
        int? PageSize,
        Func<IQueryable<Transaction>, IQueryable<Transaction>>? Includes
    ) : IRequest<PagedResult<TransactionDto>>;

    public sealed record GetTransactionByIdQuery(
        Guid Id,
        Func<IQueryable<Transaction>, IQueryable<Transaction>>? Includes
    ) : IRequest<TransactionDto?>;

    public sealed record GetTransactionsByPeriodQuery(
        DateTime startDate,
        DateTime endDate,
        Guid? categoryId = null,
        Guid? paymentMethodId = null,
        int? pageNumber = null,
        int? pageSize = null
    ) : IRequest<PagedResult<TransactionDto>>;
}
