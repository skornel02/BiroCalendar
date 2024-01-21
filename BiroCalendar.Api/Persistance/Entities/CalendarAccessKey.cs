namespace BiroCalendar.Api.Persistance.Entities;

public class CalendarAccessKey
{
    public Guid Id { get; set; }

    public int BiroAccountId { get; set; }
    public BiroAccount BiroAccount { get; set; } = default!;
}
