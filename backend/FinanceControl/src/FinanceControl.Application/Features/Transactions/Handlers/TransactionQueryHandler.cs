using FinanceControl.Application.DTOs;
using FinanceControl.Application.Features.Transactions.Queries;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using MediatR;

namespace FinanceControl.Application.Features.Transactions.Handlers
{
    public sealed class TransactionQueryHandler :
        IRequestHandler<GetAllTransactionsQuery, PagedResult<TransactionDto>>,
        IRequestHandler<GetTransactionByIdQuery, TransactionDto?>,
        IRequestHandler<GetTransactionsByPeriodQuery, PagedResult<TransactionDto>>
    {
        private readonly ITransactionService _service;

        public TransactionQueryHandler(ITransactionService service)
        {
            _service = service;
        }

        public async Task<PagedResult<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(
                request.Filters,
                request.PageNumber,
                request.PageSize,
                request.Includes
            );
        }

        public async Task<TransactionDto?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.Id, request.Includes);
        }

        public async Task<PagedResult<TransactionDto>> Handle(GetTransactionsByPeriodQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetTransactionsByPeriodAsync(
                request.startDate,
                request.endDate,
                request.categoryId,
                request.paymentMethodId,
                request.pageNumber,
                request.pageSize
            );
        }
    }
}
