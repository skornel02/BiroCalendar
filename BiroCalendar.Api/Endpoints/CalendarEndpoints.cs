using BiroCalendar.Api.Extensions;
using BiroCalendar.Api.Persistance;
using BiroCalendar.Api.Persistance.Entities;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BiroCalendar.Api.Endpoints;

public static class CalendarEndpoints
{
    public static void MapCalendarEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/calendar")
            .WithTags(nameof(CalendarAccessKey));

        group.MapGet("/", async Task<Results<FileStreamHttpResult, NotFound>> (
            [FromQuery] Guid calendarKey,
            AppDbContext db) =>
        {
            var accessKey = await db.CalendarAccessKeys
                .AsNoTracking()
                .Where(_ => _.Id == calendarKey)
                .FirstOrDefaultAsync();

            if (accessKey is null)
            {
                return TypedResults.NotFound();
            }

            var records = await db.Records
                .AsNoTracking()
                .Where(_ => _.BiroAccountId == accessKey.BiroAccountId)
                .ToListAsync();

            var calendar = records.ToCalendar();
            var serializer = new CalendarSerializer(calendar);

            var memoryStream = new MemoryStream();
            serializer.Serialize(calendar, memoryStream, Encoding.UTF8);
            memoryStream.Position = 0;

            return TypedResults.Stream(memoryStream, contentType: "text/calendar", fileDownloadName: "calendar.ics");
        });

        group.MapDelete("/", async Task<Results<NoContent, NotFound>> (
            [FromQuery] Guid calendarKey,
            AppDbContext db) =>
        {
            var deleted = await db.CalendarAccessKeys
                .Where(_ => _.Id == calendarKey)
                .ExecuteDeleteAsync();

            return deleted == 0
                ? TypedResults.NotFound()
                : TypedResults.NoContent();
        });
    }
}
