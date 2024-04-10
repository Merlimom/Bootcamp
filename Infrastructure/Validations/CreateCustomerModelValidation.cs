using Core.Constants;
using Core.Request;
using FluentValidation;
using System.Runtime.CompilerServices;

namespace Infrastructure.Validetions;

public class CreateCustomerModelValidation : AbstractValidator<CreateCustomerModel>
{
    public CreateCustomerModelValidation()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.BankId)
            .NotNull().WithMessage("Bank Id cannot be null")
            .NotEmpty().WithMessage("Bank Id cannot be empty");

        RuleFor(x => x.CustomerStatus)
            .Must(x => Enum.IsDefined(typeof(CreditCardStatus), x))
            .WithMessage("Invalid CreditCard Status");

        RuleFor(x => x.Mail)
          .EmailAddress();

        RuleFor(x => x.DocumentNumber)
            .NotNull().WithMessage("Document Number cannot be null");    
    }

    private bool BeValidCustomerStatus(int arg)
    {
        return Enum.IsDefined(typeof(CustomerStatus), arg);
    }
}