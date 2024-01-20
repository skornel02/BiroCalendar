namespace BiroCalendar.Api.Persistance.Entities;

public class BiroAccount
{
    public int Id { get; set; }
    public string AccountName { get; set; } = default!;
    public string AccountPassword { get; set; } = default!;
    public string ServiceUrl { get; set; } = default!;

    public DateTime Created {  get; set; }
    public DateTime? LastAccessed { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public List<BiroRecord> BiroRecords { get; set; } = null!;
}
