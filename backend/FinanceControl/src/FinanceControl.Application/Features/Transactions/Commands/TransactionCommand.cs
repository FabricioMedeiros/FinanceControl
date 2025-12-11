using FinanceControl.Application.DTOs;
using FinanceControl.Domain.Entities;
using MediatR;

namespace FinanceControl.Application.Features.Transactions.Commands
{
    public sealed record CreateTransactionCommand(
        TransactionDto TransactionDto,
        Func<IQueryable<Transaction>, IQueryable<Transaction>>? Includes
        ) : IRequest<TransactionDto?>;
    public sealed record UpdateTransactionCommand(TransactionDto TransactionoDto) : IRequest<Unit>;
    public sealed record DeleteTransactionCommand(Guid Id) : IRequest<Unit>;
}
