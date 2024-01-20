using BiroCalendar.Api.Persistance;
using BiroCalendar.Api.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace BiroCalendar.Api.Extensions;

public static class AccountExtension
{
    public static string? GetEmailAddress(this HttpContext context) => context?.User?.Identity?.Name;

    public static async Task<Account?> GetAccountFromEmail(this AppDbContext context, string? emailAddress)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(_ => _.EmailAddress == emailAddress);
        return account;
    }
}
