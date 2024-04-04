using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> entity)
    {
        entity
            .HasKey(e => e.Id)
            .HasName("Bank_pkey");

        entity
            .Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity
            .Property(e => e.Phone)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(e => e.Mail)
            .HasMaxLength(100)
            .IsRequired();

        entity
            .Property(e => e.Address)
            .HasMaxLength(100)
            .IsRequired();


        entity
            .HasMany(bank => bank.Customers)
            .WithOne(customer => customer.Bank)
            .HasForeignKey(costumer => costumer.BankId);
    }
}
