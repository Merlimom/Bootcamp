using Core.Constants;
using Core.Entities;
using Core.Request;
using FluentValidation;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Validations; 

public class CreateUserRequestModelValidation : AbstractValidator<CreateUserRequestModel>
{
    private readonly BootcampContext _bootcampContext; 

    public CreateUserRequestModelValidation()
    {
        RuleFor(x => x.ProductId)
               .Must(IsValidProductId).WithMessage("Invalid Product ID");

        RuleFor(x => x.CustomerId)
         .NotNull().WithMessage("Customer Id cannot be null")
         .NotEmpty().WithMessage("Customer Id cannot be empty");

        RuleFor(x => x.CurrencyId)
            .NotNull().WithMessage("Currency Id cannot be null")
            .NotEmpty().WithMessage("Currency Id cannot be empty");
    }


    //private bool IsValidProductId(int productId)
    //{
    //    int[] validProductIds = { 1, 2, 3 };

    //    return validProductIds.Contains(productId);
    //}

    private bool IsValidProductId(int productId)
    {
        var validProductIds = _bootcampContext.Products.Select(p => p.Id).ToList();
        return validProductIds.Contains(productId);
    }

}


