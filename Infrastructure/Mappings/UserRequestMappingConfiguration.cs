using Core.Entities;
using Core.Models;
using Core.Request;
using Mapster;

namespace Infrastructure.Mappings;

public class UserRequestMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserRequestModel, UserRequest>()
            .Map(dest => dest.RequestDate, src => src.RequestDate)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.CurrencyId, src => src.CurrencyId)
            .Map(dest => dest.CustomerId, src => src.CustomerId)
            .Map(dest => dest.ProductId, src => src.ProductId);

        config.NewConfig<UserRequest, UserRequestDTO>()
            .Map(dest => dest.RequestDate, src => src.RequestDate)
            .Map(dest => dest.Currency, src => src.Currency.Name)
            .Map(dest => dest.Customer, src => src.Customer.Name)
            .Map(dest => dest.Product, src => src.Product.Name);
    }
}
