using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreateUserRequestModelValidation : AbstractValidator<CreateUserRequestModel>
{
    public CreateUserRequestModelValidation()
    {
        RuleFor(x => x.ProductId)
          .NotEmpty().WithMessage("Customer Id cannot be empty");

        RuleFor(x => x.CustomerId)
             .NotEmpty().WithMessage("Customer Id cannot be empty");

        RuleFor(x => x.CurrencyId)
             .NotEmpty().WithMessage("Currency Id cannot be empty");
    }
}


