using AutoMapper;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Features.Transactions.Commands;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Notifications;
using MediatR;

namespace FinanceControl.Application.Features.Transactions.Handlers
{
    public sealed class TransactionCommandHandler :
    IRequestHandler<CreateTransactionCommand, TransactionDto?>,
    IRequestHandler<UpdateTransactionCommand, Unit>,
    IRequestHandler<DeleteTransactionCommand, Unit>
    {
        private readonly ITransactionService _service;
        private readonly INotificator _notificator;
        private readonly IMapper _mapper;

        public TransactionCommandHandler(ITransactionService service, INotificator notificator, IMapper mapper)
        {
            _service = service;
            _notificator = notificator;
            _mapper = mapper;
        }

        public async Task<TransactionDto?> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
           return await _service.AddAsync(request.TransactionDto);
        }

        public async Task<Unit> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var Transaction = await _service.GetByIdAsync(request.TransactionoDto.Id.Value, returnEntity: true);

            if (Transaction == null)
            {
                _notificator.AddNotification(new Notification("Lançamento não encontrado."));
                return Unit.Value;
            }

            _mapper.Map(request.TransactionoDto, Transaction);
            await _service.UpdateAsync(Transaction);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _service.GetByIdAsync(request.Id, returnEntity: true);
            if (entity == null)
            {
                _notificator.AddNotification(new Notification("Lançamento não encontrado."));
                return Unit.Value;
            }

            await _service.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
