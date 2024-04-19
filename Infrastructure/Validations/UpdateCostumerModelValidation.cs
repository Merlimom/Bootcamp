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
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.Mail)
          .EmailAddress();

        RuleFor(x => x.DocumentNumber)
            .NotNull().WithMessage("Document Number cannot be null");
    }

    private bool BeValidCustomerStatus(int arg)
    {
        return Enum.IsDefined(typeof(ECustomerStatus), arg);
    }
}

