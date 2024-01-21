using BiroCalendar.Api.Persistance.Configuration;
using BiroCalendar.Api.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace BiroCalendar.Api.Persistance;

public class AppDbContext : DbContext
{
    public DbSet<BiroAccount> BiroAccounts { get; set; }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<BiroRecord> Records { get; set; }

    public DbSet<CalendarAccessKey> CalendarAccessKeys { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountConfiguration).Assembly);
    }
}
