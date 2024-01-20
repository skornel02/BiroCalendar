
using BiroCalendar.Api.Clients;
using BiroCalendar.Api.Persistance;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace BiroCalendar.Api.BackgroundServices;

public class BiroAccountService : BackgroundService
{
    private readonly ILogger<BiroAccountService> _logger;
    private readonly IServiceProvider _provider;

    public BiroAccountService(
        ILogger<BiroAccountService> logger, 
        IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("BiroAccount service tick");
                var scope = _provider.CreateScope();
                var context = scope.ServiceProvider.GetService<AppDbContext>()!;
                var biroClient = scope.ServiceProvider.GetService<BiroClient>()!;

                var accounts = await context.BiroAccounts
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var account in accounts )
                {
                    try
                    {
                        await biroClient.FetchBiroRecords(account);
                    } catch ( Exception ex )
                    {
                        _logger.LogWarning(ex, "BiroAccount '{accountName}' failed to update!", account.AccountName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while running BiroAccount service");
            }

            await Task.Delay(1000 * 60 * 60 * 24);
        }
    }
}
