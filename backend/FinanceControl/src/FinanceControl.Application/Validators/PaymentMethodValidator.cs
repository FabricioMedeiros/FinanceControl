using FinanceControl.Application.DTOs;
using FluentValidation;

namespace FinanceControl.Application.Validators
{
    public abstract class PaymentMethodValidatorBase : AbstractValidator<PaymentMethodDto>
    {
        public PaymentMethodValidatorBase()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do método de pagamento é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.");
        }
    }

    public class PaymentMethodCreateValidator : PaymentMethodValidatorBase
    {
        public PaymentMethodCreateValidator() { }
    }

    public class PaymentMethodUpdateValidator : PaymentMethodValidatorBase
    {
        public PaymentMethodUpdateValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");
        }
    }
}
