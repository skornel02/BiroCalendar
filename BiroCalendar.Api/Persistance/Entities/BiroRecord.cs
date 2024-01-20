namespace BiroCalendar.Api.Persistance.Entities;

public class BiroRecord
{
    public int Id { get; set; }

    public string ClassName { get; set; } = default!;
    public string TaskName { get; set; } = default!;
    public DateTime TaskDueDate { get; set; }

    public bool Outdated { get; set; }
    public DateTime FetchedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? OutdatedAt { get; set; }

    public int BiroAccountId { get; set; }
    public BiroAccount BiroAccount { get; set; } = default!;
}
