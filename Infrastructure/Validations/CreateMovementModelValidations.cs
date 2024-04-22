using Core.Constants;
using Core.Entities;
using Core.Request;
using FluentValidation;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Validations;

public class CreateMovementModelValidations : AbstractValidator<CreateMovementModel>
{
    public CreateMovementModelValidations()
    {
        RuleFor(x => x.MovementType)
           .NotNull().WithMessage("Movement Type cannot be null")
           .Must(movementType => Enum.IsDefined(typeof(EMovementType), movementType))
           .WithMessage("Invalid Movement Type");

        When(x => x.MovementType == EMovementType.Transfer, () =>
        {
            RuleFor(x => x.AccountDestinationId)
                .NotEmpty().WithMessage("Account Destination ID is required for transfer");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero for transfer");
        });

        When(x => x.MovementType == EMovementType.Deposit, () =>
        {
            RuleFor(x => x.AccountDestinationId)
                .NotEmpty().WithMessage("Account Destination ID is required for deposit");

            RuleFor(x => x.DestinationBankId)
                .NotEmpty().WithMessage("Destination Bank ID is required for deposit");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero for deposit");
        });

        When(x => x.MovementType == EMovementType.Withdrawal, () =>
        {
            RuleFor(x => x.AccountDestinationId)
                .NotEmpty().WithMessage("Account Destination ID is required for withdrawal");

            RuleFor(x => x.DestinationBankId)
                .NotEmpty().WithMessage("Destination Bank ID is required for withdrawal");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero for withdrawal");
        });
    }   
}
