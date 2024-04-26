using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class UpdateCurrencyModelValidation : AbstractValidator<UpdateCurrencyModel>
{
    public UpdateCurrencyModelValidation()
    {
        RuleFor(x => x.Name)
          .NotNull().WithMessage("Name cannot be null")
          .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.BuyValue)
           .NotNull().WithMessage("BuyValue cannot be null")
           .NotEmpty().WithMessage("BuyValue cannot be empty")
           .GreaterThan(0).WithMessage("BuyValue must be greater than zero.");


        RuleFor(x => x.SellValue)
           .NotNull().WithMessage("SellValue cannot be null")
           .NotEmpty().WithMessage("SellValue cannot be empty")
           .GreaterThan(0).WithMessage("SellValue must be greater than zero.");
    }
}
