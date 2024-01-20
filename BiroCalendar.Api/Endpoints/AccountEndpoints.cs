using BiroCalendar.Api.Extensions;
using BiroCalendar.Api.Persistance;
using BiroCalendar.Api.Persistance.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BiroCalendar.Api.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/account")
            .WithTags(nameof(Account))
            .AllowAnonymous();

        group.MapGet("/", async Task<Results<Ok<string>, UnauthorizedHttpResult>> (
            [FromServices] AppDbContext db,
            HttpContext context) =>
        {
            var username = context.GetEmailAddress();
            if (username is null)
            {
                return TypedResults.Unauthorized();
            }

            return TypedResults.Ok(username);
        })
            .WithName("Login status")
            .WithOpenApi();

        group.MapPost("/login", async Task<Results<Accepted, BadRequest<string>>> (
            [FromQuery] string email,
            [FromQuery] string password,
            [FromServices] AppDbContext db,
            HttpContext context,
            [FromServices] IPasswordHasher<Account> hasher) =>
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return TypedResults.BadRequest("Bad credentails");
            }

            var account = await db.Accounts.FirstOrDefaultAsync(_ => _.EmailAddress == email);

            if (account is null)
            {
                return TypedResults.BadRequest("No account with email");
            }

            if (hasher.VerifyHashedPassword(account, account.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                return TypedResults.BadRequest("Invalid password");
            }

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, email)
            };
            var principle = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);

            return TypedResults.Accepted("/account");
        })
            .WithName("Login")
            .WithOpenApi();

        group.MapPost("/logout", async (
            [FromServices] AppDbContext db,
            HttpContext context) =>
        {
            await context.SignOutAsync();

            return Results.NoContent();
        })
            .WithName("Logout")
            .WithOpenApi();

        group.MapPost("/register", async Task<Results<Accepted, BadRequest<string>>> (
            [FromQuery] string email,
            [FromQuery] string password,
            [FromServices] AppDbContext db,
            [FromServices] IPasswordHasher<Account> hasher) =>
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return TypedResults.BadRequest("invalid credentials");
            }

            var existingAccount = await db.Accounts.FirstOrDefaultAsync(_ => _.EmailAddress == email);

            if (existingAccount is not null)
            {
                return TypedResults.BadRequest("account already exists");
            }

            var account = new Account()
            {
                EmailAddress = email,
            };
            account.PasswordHash = hasher.HashPassword(account, password);

            await db.AddAsync(account);
            await db.SaveChangesAsync();

            return TypedResults.Accepted("/account/login");
        })
            .WithName("Register")
            .WithOpenApi();
    }
}
