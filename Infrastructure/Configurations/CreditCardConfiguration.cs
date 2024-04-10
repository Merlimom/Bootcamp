using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> entity)
    {
        entity
            .ToTable("CreditCards");
        entity
           .HasKey(e => e.Id)
           .HasName("CreditCard_pkey");

        entity
            .Property(e => e.Designation)
            .HasMaxLength(100).IsRequired();

        entity
            .Property(e => e.IssueDate);


        entity
            .Property(e => e.ExpirationDate);
            

        entity
            .Property(e => e.CardNumber)
            .HasMaxLength(20).IsRequired();

        entity
            .Property(e => e.Cvv)
            .HasMaxLength(10).IsRequired();

        entity
          .Property(e => e.CreditLimit)
          .HasPrecision(20, 5).IsRequired();


        entity
            .Property(e => e.AvailableCredit)
            .HasPrecision(20, 5).IsRequired();

        entity
           .Property(e => e.CurrentDebt)
           .HasPrecision(20, 5);

        entity
           .Property(e => e.InterestRate)
           .HasPrecision(20, 5).IsRequired();



        entity.HasOne(d => d.Customer)
              .WithMany(p => p.CreditCards)
              .HasForeignKey(d => d.CustomerId);


        entity.HasOne(d => d.Currency)
          .WithMany(p => p.CreditCards)
          .HasForeignKey(d => d.CurrencyId);
    }
}
