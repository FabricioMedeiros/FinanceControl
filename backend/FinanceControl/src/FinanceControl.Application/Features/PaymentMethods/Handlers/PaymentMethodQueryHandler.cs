using FinanceControl.Application.DTOs;
using FinanceControl.Application.Features.PaymentMethods.Queries;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using MediatR;

namespace FinanceControl.Application.Features.PaymentMethods.Handlers
{
    public sealed class PaymentMethodQueryHandler :
        IRequestHandler<GetAllPaymentMethodsQuery, PagedResult<PaymentMethodDto>>,
        IRequestHandler<GetPaymentMethodByIdQuery, PaymentMethodDto?>
    {
        private readonly IPaymentMethodService _service;

        public PaymentMethodQueryHandler(IPaymentMethodService service)
        {
            _service = service;
        }

        public async Task<PagedResult<PaymentMethodDto>> Handle(GetAllPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(
                request.Filters,
                request.PageNumber,
                request.PageSize
            );
        }

        public async Task<PaymentMethodDto?> Handle(GetPaymentMethodByIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.Id);
        }
    }
}
