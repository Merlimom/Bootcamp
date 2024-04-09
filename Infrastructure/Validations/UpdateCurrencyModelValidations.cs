using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class UpdateCurrencyModelValidations : AbstractValidator<UpdateCurrencyModel>
{
    public UpdateCurrencyModelValidations()
    {
        RuleFor(x => x.Name)
          .NotNull().WithMessage("Name cannot be null")
          .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.BuyValue)
           .NotNull().WithMessage("Name cannot be null")
           .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.SellValue)
           .NotNull().WithMessage("Name cannot be null")
           .NotEmpty().WithMessage("Name cannot be empty");
    }
}
