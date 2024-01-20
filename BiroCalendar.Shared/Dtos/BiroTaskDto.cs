namespace BiroCalendar.Shared.Dtos;

public record BiroTaskDto(
        string ClassName,
        string TaskName,
        DateTime TaskDueDate,
        DateTime FetchedAt,
        DateTime UpdatedAt
);
