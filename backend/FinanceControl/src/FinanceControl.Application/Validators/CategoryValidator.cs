using FinanceControl.Application.DTOs;
using FluentValidation;

namespace FinanceControl.Application.Validators
{
    public abstract class CategoryValidatorBase : AbstractValidator<CategoryDto>
    {
        public CategoryValidatorBase()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.");

            RuleFor(x => x.Type)
                .NotNull().WithMessage("O tipo da categoria é obrigatório.");
        }
    }

    public class CategoryCreateValidator : CategoryValidatorBase
    {
        public CategoryCreateValidator() { }
    }

    public class CategoryUpdateValidator : CategoryValidatorBase
    {
        public CategoryUpdateValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");
        }
    }
}
