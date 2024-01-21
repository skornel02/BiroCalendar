using BiroCalendar.Api.Persistance.Entities;
using BiroCalendar.Shared.Dtos;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace BiroCalendar.Api.Extensions;

public static class DtoMapper
{
    public static IEnumerable<BiroAccountDto> ToDto(this IEnumerable<BiroAccount> list)
        => list.Select(ToDto);

    public static BiroAccountDto ToDto(this BiroAccount entity)
    {
        return new()
        {
            Id = entity.Id,
            AccountName = entity.AccountName,
            ServiceUrl = entity.ServiceUrl,
        };
    }

    public static IEnumerable<BiroAccount?> ToEntity(this IEnumerable<BiroAccountCreationDto> list)
        => list.Select(ToEntity);

    public static BiroAccount? ToEntity(this BiroAccountCreationDto dto)
    {
        if (!dto.IsValidCreation)
        {
            return null;
        }

        return new()
        {
            AccountName = dto.AccountName,
            ServiceUrl = dto.ServiceUrl,
            AccountPassword = dto.AccountPassword,
        };
    }

    public static BiroTaskDto ToDto(this BiroRecord record)
        => new(record.ClassName, record.TaskName, record.TaskDueDate, record.FetchedAt, record.UpdatedAt);

    public static List<BiroTaskDto> ToDto(this ICollection<BiroRecord> records)
        => records.Select(ToDto).ToList();

    public static BiroTaskContainerDto ToContainerDto(this List<BiroTaskDto> tasks, DateTime lastAccessed)
        => new(lastAccessed, tasks);

    public static Calendar ToCalendar(this ICollection<BiroRecord> tasks)
    {
        var calendar = new Calendar();
        calendar.AddTimeZone("Europe/Budapest");
        calendar.ProductId = "BiroCalendar";
        
        foreach (var task in tasks)
        {
            var startDate = new CalDateTime(task.TaskDueDate);
            startDate.HasTime = false;

            var endDate = new CalDateTime(task.TaskDueDate.AddDays(1));
            endDate.HasTime = false;

            calendar.Events.Add(new CalendarEvent()
            {
                Uid = task.Guid.ToString(),
                IsAllDay = true,
                DtStart = startDate,
                DtEnd = endDate,
                Created = new CalDateTime(task.FetchedAt),
                LastModified = new CalDateTime(task.UpdatedAt),
                Description = "Automatically imported from BiroCalendar",
                Summary = $"[{task.ClassName}] {task.TaskName}"
            });
        }

        return calendar;
    }
}
