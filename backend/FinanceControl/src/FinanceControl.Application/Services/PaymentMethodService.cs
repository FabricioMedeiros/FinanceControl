using AutoMapper;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;

namespace FinanceControl.Application.Services
{
    public class PaymentMethodService : GenericService<PaymentMethod, PaymentMethodDto>, IPaymentMethodService
    {
        public PaymentMethodService(
            IUnitOfWork uow,
            IGenericRepository<PaymentMethod> repository,
            IMapper mapper,
            INotificator notificator)
            : base(uow, repository, mapper, notificator)
        {
        }
    }
}
