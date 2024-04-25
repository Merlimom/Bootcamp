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
        RuleFor(x => x.Amount)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty");
    }   
}
