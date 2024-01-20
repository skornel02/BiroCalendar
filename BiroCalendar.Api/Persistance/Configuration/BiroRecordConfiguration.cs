using BiroCalendar.Api.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiroCalendar.Api.Persistance.Configuration;

public class BiroRecordConfiguration : IEntityTypeConfiguration<BiroRecord>
{
    public void Configure(EntityTypeBuilder<BiroRecord> builder)
    {
        builder.HasOne(_ => _.BiroAccount)
            .WithMany(_ => _.BiroRecords)
            .HasForeignKey(_ => _.BiroAccountId);
    }
}
