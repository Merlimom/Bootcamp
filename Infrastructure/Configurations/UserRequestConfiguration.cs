using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserRequestConfiguration : IEntityTypeConfiguration<UserRequest>
{
    public void Configure(EntityTypeBuilder<UserRequest> entity)
    {

        entity
            .HasKey(e => e.Id)
            .HasName("UserRequest_pkey");

        entity
            .Property(e => e.RequestDate)
            .IsRequired();

        entity
            .Property(e => e.ApprovalDate)
            .IsRequired();

        entity
            .HasOne(userRequest => userRequest.Currency)
            .WithMany(currency => currency.UserRequests)
            .HasForeignKey(userRequest => userRequest.CurrencyId);

        entity
            .HasOne(userRequest => userRequest.Customer)
            .WithMany(customer => customer.UserRequests)
            .HasForeignKey(userRequest => userRequest.CustomerId);

        entity
           .HasOne(userRequest => userRequest.Product)
           .WithMany(product => product.UserRequests)
           .HasForeignKey(userRequest => userRequest.ProductId);
    }
}
