using BiroCalendar.Api.Persistance.Entities;
using BiroCalendar.Shared.Dtos;

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
}
