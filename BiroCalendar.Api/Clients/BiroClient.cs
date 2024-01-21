using BiroCalendar.Api.Helpers;
using BiroCalendar.Api.Persistance;
using BiroCalendar.Api.Persistance.Entities;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace BiroCalendar.Api.Clients;

public class BiroClient
{
    private readonly ILogger<BiroClient> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppDbContext _context;

    public BiroClient(
        ILogger<BiroClient> logger,
        IHttpClientFactory httpClientFactory, 
        AppDbContext context)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    public async Task<List<BiroRecord>> FetchBiroRecords(BiroAccount account)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("biro");
            client.BaseAddress = new(account.ServiceUrl);

            await client.GetAsync("beleptet.php");

            var content = new MultipartFormDataContent
                        {
                            { new StringContent(account.AccountName), "loginnev" },
                            { new StringContent(account.AccountPassword), "passwd" },
                            { new StringContent("0"), "x" },
                            { new StringContent("0"), "y" }
                        };


            var loginResponse = await client.PostAsync("beleptet.php", content);

            var success = !(loginResponse.RequestMessage?.RequestUri?.AbsolutePath == "/beleptet.php");

            var resp = await loginResponse.Content.ReadAsStringAsync();

            if (!success)
            {
                throw new Exception("Login failed!");
            }

            var data = await client.GetStringAsync("feladat_keres.php");
            var document = new HtmlDocument();
            document.LoadHtml(data);

            var node = document.DocumentNode;
            var rows = node.Descendants("tr");

            var records = new List<BiroRecord>();

            var existingRecords = await _context.Records
                .AsNoTracking()
                .Where(_ => _.BiroAccountId == account.Id)
                .Where(_ => !_.Outdated)
                .ToListAsync();

            var seenExistingRecords = new List<BiroRecord>();
            var newRecords = new List<BiroRecord>();

            var className = "-";
            foreach (var row in rows.Skip(1))
            {
                var columns = row.Descendants("td").ToList();
                if (columns.Count < 6)
                {
                    _logger.LogWarning("Invalid row found for '{username}'! '{row}'", account.AccountName, row.InnerHtml);
                }

                if (columns.Count == 7)
                {
                    var classColumnData = columns.First();
                    className = classColumnData.InnerText.Trim();

                    columns = columns.Skip(1).ToList();
                }

                var taskName = columns[0].InnerText.Trim();
                var taskDueDate = DateTime.Parse(columns[3].InnerText.Trim());

                var existingRecord = existingRecords
                    .FirstOrDefault(_ => _.TaskName == taskName && _.TaskDueDate == taskDueDate);

                if (existingRecord is not null)
                {
                    seenExistingRecords.Add(existingRecord);
                } else
                {
                    var record = new BiroRecord
                    {
                        Guid = Guid.NewGuid(),
                        ClassName = className,
                        TaskName = taskName,
                        TaskDueDate = taskDueDate,
                        FetchedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Outdated = false, 
                        BiroAccountId = account.AccountId,
                    };

                    newRecords.Add(record);
                }
            }

            var updateOutdateFilter = PredicateBuilder.True<BiroRecord>();
            var updateNotOutdatedFilter = PredicateBuilder.False<BiroRecord>();

            foreach (var seenRecord in seenExistingRecords)
            {
                updateOutdateFilter = updateOutdateFilter.And(_ => _.Id != seenRecord.Id);
                updateNotOutdatedFilter = updateNotOutdatedFilter.Or(_ => _.Id == seenRecord.Id);
            }

            await _context.Records
                .Where(updateOutdateFilter)
                .ExecuteUpdateAsync(_ => _.SetProperty(__ => __.Outdated, true)
                    .SetProperty(__ => __.OutdatedAt, DateTime.Now));

            await _context.Records
                .Where(updateNotOutdatedFilter)
                .ExecuteUpdateAsync(_ => _.SetProperty(__ => __.UpdatedAt, DateTime.Now));

            await _context.Records.AddRangeAsync(newRecords);
            await _context.SaveChangesAsync();

            await _context.BiroAccounts
                .Where(_ => _.Id == account.Id)
                .ExecuteUpdateAsync(_ => _.SetProperty(__ => __.LastAccessed, DateTime.Now));

            return seenExistingRecords.Concat(newRecords).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "BiroAccount '{accountName}' failed to fetch!", account.AccountName);
            throw;
        }
    }
}
