using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity
            .ToTable("Products");

        entity
           .HasKey(e => e.Id)
           .HasName("Product_pkey");

        entity
            .Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
