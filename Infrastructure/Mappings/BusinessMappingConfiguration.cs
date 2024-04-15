using Core.Entities;
using Core.Models;
using Core.Request;
using Mapster;

namespace Infrastructure.Mappings;

public class BusinessMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<CreateBusinessModel, Business>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.Phone, src => src.Phone)
            .Map(dest => dest.Email, src => src.Email);
            

        config.NewConfig<Business, BusinessDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.Phone, src => src.Phone)
            .Map(dest => dest.Email, src => src.Email);
    }
}
