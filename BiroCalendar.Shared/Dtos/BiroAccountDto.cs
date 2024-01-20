namespace BiroCalendar.Shared.Dtos;

public class BiroAccountDto
{
    public required int Id { get; set; }
    public required string AccountName { get; set; } = default!;
    public required string ServiceUrl { get; set; } = default!;
}
