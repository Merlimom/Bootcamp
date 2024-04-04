using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class MovementConfiguration : IEntityTypeConfiguration<Movement>
{
    public void Configure(EntityTypeBuilder<Movement> entity)
    {

        entity
            .HasKey(e => e.Id)
            .HasName("Movement_pkey");

        entity
            .Property(e => e.Destination)
            .HasMaxLength(100).IsRequired();

        entity
            .Property(e => e.Amount)
            .HasPrecision(20, 5);

        entity
            .Property(e => e.TransferredDateTime);
            


        entity.HasOne(d => d.Account)
            .WithMany(p => p.Movements)
            .HasForeignKey(d => d.AccountId);
            
    }
}
