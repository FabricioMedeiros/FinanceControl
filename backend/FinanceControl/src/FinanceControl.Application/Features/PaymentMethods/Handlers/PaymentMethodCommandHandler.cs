using AutoMapper;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Features.PaymentMethods.Commands;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Notifications;
using MediatR;

namespace FinanceControl.Application.Features.PaymentMethods.Handlers
{
    public sealed class PaymentMethodCommandHandler :
    IRequestHandler<CreatePaymentMethodCommand, PaymentMethodDto?>,
    IRequestHandler<UpdatePaymentMethodCommand, Unit>,
    IRequestHandler<DeletePaymentMethodCommand, Unit>
    {
        private readonly IPaymentMethodService _service;
        private readonly INotificator _notificator;
        private readonly IMapper _mapper;

        public PaymentMethodCommandHandler(IPaymentMethodService service, INotificator notificator, IMapper mapper)
        {
            _service = service;
            _notificator = notificator;
            _mapper = mapper;
        }

        public async Task<PaymentMethodDto?> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var exists = await _service.ExistsAsync(c => c.Name.Trim() == request.PaymentMethodDto.Name.Trim());
            if (exists)
            {
                _notificator.AddNotification(new Notification("Já existe uma forma de pagamento com esse nome."));
                return null!;
            }

            return await _service.AddAsync(request.PaymentMethodDto);
        }

        public async Task<Unit> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var PaymentMethod = await _service.GetByIdAsync(request.PaymentMethodDto.Id.Value, returnEntity: true);

            if (PaymentMethod == null)
            {
                _notificator.AddNotification(new Notification("Forma de pagamento não encontrada."));
                return Unit.Value;
            }

            _mapper.Map(request.PaymentMethodDto, PaymentMethod);
            await _service.UpdateAsync(PaymentMethod);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var entity = await _service.GetByIdAsync(request.Id, returnEntity: true);
            if (entity == null)
            {
                _notificator.AddNotification(new Notification("Forma de pagamento não encontrada."));
                return Unit.Value;
            }

            await _service.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
