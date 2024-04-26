using Core.Constants;
using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations; 

public class UpdateCostumerModelValidation : AbstractValidator<UpdateCustomerModel>
{
    public UpdateCostumerModelValidation()
    {
        RuleFor(x => x.Name)
           .NotNull().WithMessage("Name cannot be null")
           .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.BankId)
            .NotEmpty().WithMessage("BankId cannot be empty");

        RuleFor(x => x.CustomerStatus)
           .Must(x => Enum.IsDefined(typeof(ECustomerStatus), x))
           .WithMessage("Invalid Customer Status");

        RuleFor(x => x.Mail)
          .EmailAddress();

        RuleFor(x => x.DocumentNumber)
            .NotNull().WithMessage("Document Number cannot be null");
    }
}

