namespace BiroCalendar.Shared.Dtos;

public record BiroTaskContainerDto(
    DateTime LastAccessed,
    List<BiroTaskDto> Tasks
);
