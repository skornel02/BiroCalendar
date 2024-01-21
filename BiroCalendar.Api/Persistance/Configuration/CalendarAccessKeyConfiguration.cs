using BiroCalendar.Api.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiroCalendar.Api.Persistance.Configuration;

public class CalendarAccessKeyConfiguration : IEntityTypeConfiguration<CalendarAccessKey>
{
    public void Configure(EntityTypeBuilder<CalendarAccessKey> builder)
    {
        builder.HasKey(_ => _.Id);

        builder.HasOne(_ => _.BiroAccount)
            .WithMany()
            .HasForeignKey(_ => _.BiroAccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
