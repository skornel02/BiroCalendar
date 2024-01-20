using BiroCalendar.Api.Clients;
using BiroCalendar.Api.Extensions;
using BiroCalendar.Api.Persistance;
using BiroCalendar.Api.Persistance.Entities;
using BiroCalendar.Shared.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiroCalendar.Api.Endpoints;

public static class BiroAccountEndpoints
{
    public static void MapBiroAccountEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/BiroAccount")
            .WithTags(nameof(BiroAccount));

        group.MapGet("/", async Task<Results<Ok<IEnumerable<BiroAccountDto>>, UnauthorizedHttpResult>> (
            AppDbContext db,
            HttpContext context) =>
        {
            var account = await db.GetAccountFromEmail(context.GetEmailAddress());

            if (account is null)
            {
                return TypedResults.Unauthorized();
            }

            var accounts = await db.BiroAccounts
                .Where(_ => _.AccountId == account.Id)
                .ToListAsync();

            return TypedResults.Ok(accounts.ToDto());
        })
            .WithName("GetAllBiroAccounts")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapGet("/{id}", async Task<Results<Ok<BiroAccountDto>, NotFound, UnauthorizedHttpResult>> (
            int id,
            AppDbContext db,
            HttpContext context) =>
        {
            var account = await db.GetAccountFromEmail(context.GetEmailAddress());

            if (account is null)
            {
                return TypedResults.Unauthorized();
            }

            return await db.BiroAccounts.AsNoTracking()
                .Where(_ => _.AccountId == account.Id)
                .FirstOrDefaultAsync(model => model.Id == id)
                is BiroAccount model
                    ? TypedResults.Ok(model.ToDto())
                    : TypedResults.NotFound();
        })
            .WithName("GetBiroAccountById")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> (
            int id,
            BiroAccount biroAccount,
            AppDbContext db,
            HttpContext context) =>
        {
            var account = await db.GetAccountFromEmail(context.GetEmailAddress());

            if (account is null)
            {
                return TypedResults.Unauthorized();
            }

            var affected = await db.BiroAccounts
                .Where(_ => _.AccountId == account.Id)
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, biroAccount.Id)
                  .SetProperty(m => m.AccountName, biroAccount.AccountName)
                  .SetProperty(m => m.AccountPassword, biroAccount.AccountPassword)
                  .SetProperty(m => m.ServiceUrl, biroAccount.ServiceUrl)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
            .WithName("UpdateBiroAccount")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapPost("/", async Task<Results<Created<BiroAccountDto>, BadRequest, UnauthorizedHttpResult>> (
            BiroAccountCreationDto dto,
            AppDbContext db,
            HttpContext context) =>
        {
            var account = await db.GetAccountFromEmail(context.GetEmailAddress());

            if (account is null)
            {
                return TypedResults.Unauthorized();
            }

            var biroAccount = dto.ToEntity();

            if (biroAccount is null)
            {
                return TypedResults.BadRequest();
            }

            biroAccount.AccountId = account.Id;

            await db.BiroAccounts.AddAsync(biroAccount);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/api/BiroAccount/{biroAccount.Id}", biroAccount.ToDto());
        })
            .WithName("CreateBiroAccount")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> (
            int id,
            AppDbContext db,
            HttpContext context) =>
        {
            var account = await db.GetAccountFromEmail(context.GetEmailAddress());

            if (account is null)
            {
                return TypedResults.Unauthorized();
            }

            var affected = await db.BiroAccounts
                .Where(_ => _.AccountId == account.Id)
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
            .WithName("DeleteBiroAccount")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapGet("/{id}/tasks", async Task<Results<Ok<BiroTaskContainerDto>, NotFound, UnauthorizedHttpResult>> (
            int id,
            AppDbContext db,
            HttpContext context,
            [FromQuery] bool showAll = false) =>
        {
            var account = await db.GetAccountFromEmail(context.GetEmailAddress());

            if (account is null)
            {
                return TypedResults.Unauthorized();
            }

            var biroAccount = await db.BiroAccounts
                .AsNoTracking()
                .Where(_ => _.AccountId == id)
                .FirstOrDefaultAsync();

            if (biroAccount is null)
            {
                return TypedResults.NotFound();
            }

            var records = await db.Records
                .AsNoTracking()
                .Where(_ => _.BiroAccountId == id)
                .Where(_ => !_.Outdated || showAll)
                .ToListAsync();

            return TypedResults.Ok(records.ToDto().ToContainerDto(biroAccount.LastAccessed ?? DateTime.MinValue));
        });

        group.MapPost("/{id}/tasks/refresh", async Task<Results<Ok<BiroTaskContainerDto>, NotFound, UnauthorizedHttpResult>> (
            int id,
            AppDbContext db,
            HttpContext context,
            BiroClient biroClient) =>
        {
            var account = await db.GetAccountFromEmail(context.GetEmailAddress());

            if (account is null)
            {
                return TypedResults.Unauthorized();
            }

            var biroAccount = await db.BiroAccounts.AsNoTracking()
                .Where(_ => _.AccountId == account.Id)
                .FirstOrDefaultAsync(model => model.Id == id);

            if (biroAccount is null)
            {
                return TypedResults.NotFound();
            }

            var records = await biroClient.FetchBiroRecords(biroAccount);

            return TypedResults.Ok(records.ToDto().ToContainerDto(biroAccount.LastAccessed ?? DateTime.MinValue));
        });
    }
}
