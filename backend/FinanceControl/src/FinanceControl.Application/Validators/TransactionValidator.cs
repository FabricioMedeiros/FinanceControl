using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FluentValidation;

namespace FinanceControl.Application.Validators
{
    public abstract class TransactionValidatorBase : AbstractValidator<TransactionDto>
    {
        private readonly ICategoryService _categoryService;
        private readonly IPaymentMethodService _paymentMethodService;
        
        public TransactionValidatorBase(
            ICategoryService categoryService,
            IPaymentMethodService paymentMethodService)
        {
            _categoryService = categoryService;
            _paymentMethodService = paymentMethodService;
           
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("A data da transação é obrigatória.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data não pode ser no futuro.");

            RuleFor(x => x.Category)
                .NotNull().WithMessage("A categoria da transação é obrigatória.")
                .MustAsync(async (category, cancellationToken) =>
                    await ValidateCategoryAsync(category?.Id ?? Guid.Empty, cancellationToken))
                .WithMessage("A categoria não existe no banco de dados.");

            RuleFor(x => x.PaymentMethod)
                .NotNull().WithMessage("O método de pagamento da transação é obrigatória.")
                .MustAsync(async (paymentMethod, cancellationToken) =>
                    await ValidatePaymentMethodAsync(paymentMethod?.Id ?? Guid.Empty, cancellationToken))
                .WithMessage("O método de pagamento não existe no banco de dados.");
        }

        private async Task<bool> ValidateCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
        {        
            var category = await _categoryService.GetByIdAsync(categoryId, true);
            return category != null;
        }

        private async Task<bool> ValidatePaymentMethodAsync(Guid paymentMethodId, CancellationToken cancellationToken)
        {
            var paymentMethod = await _paymentMethodService.GetByIdAsync(paymentMethodId, true);
            return paymentMethod != null;
        }
    }

    public class TransactionCreateValidator : TransactionValidatorBase
    {
        public TransactionCreateValidator(
            ICategoryService categoryService,
            IPaymentMethodService paymentMethodService)
            : base(categoryService, paymentMethodService)
        {
        }
    }

    public class TransactionUpdateValidator : TransactionValidatorBase
    {
        public TransactionUpdateValidator(
            ICategoryService categoryService,
            IPaymentMethodService paymentMethodService)
            : base(categoryService, paymentMethodService)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");
        }
    }
}
