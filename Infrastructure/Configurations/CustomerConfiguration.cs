using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Customer> entity)
    {
        entity
            .HasKey(e => e.Id)
            .HasName("Customer_pkey");

        entity
            .Property(e => e.Name)
            .HasMaxLength(100).IsRequired();

        entity
            .Property(e => e.Lastname)
            .HasMaxLength(100);

        entity
            .Property(e => e.DocumentNumber)
            .HasMaxLength(100).IsRequired();

        entity
            .Property(e => e.Address)
            .HasMaxLength(100);

        entity
            .Property(e => e.Mail)
            .HasMaxLength(100);

        entity
            .Property(e => e.Phone)
            .HasMaxLength(100);


        entity.HasOne(d => d.Bank)
              .WithMany(p => p.Customers)
              .HasForeignKey(d => d.BankId);
    }
}
