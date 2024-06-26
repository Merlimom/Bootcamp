﻿using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreateDepositModelValidation : AbstractValidator<CreateDepositModel>
{
    public CreateDepositModelValidation()
    {
        RuleFor(x => x.AccountId)
           .NotNull().WithMessage("AccountId cannot be null")
           .NotEmpty().WithMessage("AccountId cannot be empty");

        RuleFor(x => x.BankId)
          .NotNull().WithMessage("BankId cannot be null")
          .NotEmpty().WithMessage("BankId cannot be empty");


        RuleFor(x => x.Amount)
         .NotNull().WithMessage("Amount cannot be null")
         .NotEmpty().WithMessage("Amount cannot be empty")
         .GreaterThan(0).WithMessage("Credit Limit must be greater than zero.");
    }
}
