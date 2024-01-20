using BiroCalendar.Api.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiroCalendar.Api.Persistance.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasIndex(_ => _.EmailAddress)
            .IsUnique();

        builder.HasMany(_ => _.BiroAccounts)
            .WithOne(_ => _.Account)
            .HasForeignKey(_ => _.AccountId);
    }
}
