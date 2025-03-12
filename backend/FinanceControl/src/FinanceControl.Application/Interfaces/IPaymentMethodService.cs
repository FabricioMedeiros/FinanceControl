using FinanceControl.Application.DTOs;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Interfaces
{
    public interface IPaymentMethodService : IGenericService<PaymentMethod, PaymentMethodDto>
    {
    }
}
