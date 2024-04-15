using Core.Entities;
using Core.Models;
using Core.Request;
using Mapster;

namespace Infrastructure.Mappings;

public class PromotionMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreatePromotionModel, Promotion>()
           .Map(dest => dest.Name, src => src.Name)
           .Map(dest => dest.DurationTime, src => src.DurationTime)
           .Map(dest => dest.PercentageOff, src => src.PercentageOff)
           .Map(dest => dest.BusinessId, src => src.BusinessId);


        config.NewConfig<Promotion, PromotionDTO>()
          .Map(dest => dest.Id, src => src.Id)
          .Map(dest => dest.Name, src => src.Name)
          .Map(dest => dest.DurationTime, src => src.DurationTime)
          .Map(dest => dest.PercentageOff, src => src.PercentageOff)
          .Map(dest => dest.Business, src => src.Business);
    }
}
