using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> entity)
    {
        entity
            .HasKey(e => e.Id)
            .HasName("Account_pkey");

        entity
            .Property(e => e.Number)
            .HasMaxLength(100)
            .IsRequired();

        entity
            .Property(e => e.Balance)
            .HasPrecision(20, 5);

       

        entity
            .HasOne(account => account.Currency)
            .WithMany(currency => currency.Accounts)
            .HasForeignKey(account => account.CurrencyId);

        entity
            .HasOne(account => account.Customer)
            .WithMany(customer => customer.Accounts)
            .HasForeignKey(account => account.CustomerId);

        entity
            .HasMany(account => account.Movements)
            .WithOne(movement => movement.Account)
            .HasForeignKey(movement => movement.AccountDestinationId);


        entity
            .HasMany(account => account.Movements)
            .WithOne(movement => movement.Account)
            .HasForeignKey(movement => movement.AccountSourceId);

        entity
           .HasMany(account => account.Payments)
           .WithOne(payment => payment.Account)
           .HasForeignKey(payment => payment.AccountId);

        entity
          .HasMany(account => account.Deposits)
          .WithOne(deposit => deposit.Account)
          .HasForeignKey(deposit => deposit.AccountId);

        entity
            .HasOne(account => account.SavingAccount)
            .WithOne(savingAccount => savingAccount.Account)
            .HasForeignKey<SavingAccount>(savingAccount => savingAccount.AccountId);


        entity
            .HasOne(account => account.CurrentAccount)
            .WithOne(savingAccount => savingAccount.Account)
            .HasForeignKey<CurrentAccount>(savingAccount => savingAccount.AccountId);

    }
}
