using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> entity)
    {
        entity.ToTable("Currencies");
        entity
            .HasKey(e => e.Id)
            .HasName("Currency_pkey");

        entity
            .Property(e => e.Name)
            .HasMaxLength(100).IsRequired();

        entity
            .Property(e => e.BuyValue)
            .HasMaxLength(100).IsRequired();

        entity
            .Property(e => e.SellValue)
            .HasMaxLength(100).IsRequired();
    }
}
