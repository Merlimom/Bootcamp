using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations;

public class PromotionEnterpriseConfiguration : IEntityTypeConfiguration<PromotionEnterprise>
{
    public void Configure(EntityTypeBuilder<PromotionEnterprise> entity)
    {
        entity.HasKey(e => new { e.PromotionId, e.EnterpriseId });

        entity
            .HasOne(e => e.Promotion)
            .WithMany(e => e.PromotionsEnterprises)
            .HasForeignKey(e => e.PromotionId);

        entity
            .HasOne(e => e.Enterprise)
            .WithMany(e => e.PromotionsEnterprises)
            .HasForeignKey(e => e.EnterpriseId);
    }
}
