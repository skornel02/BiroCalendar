using BiroCalendar.Api.Persistance.Entities;
using Microsoft.AspNetCore.Identity;

namespace BiroCalendar.Api.Services;

public class AccountPasswordHasher : PasswordHasher<Account>
{
    public PasswordVerificationResult VerifyPassword(Account account, string password)
        => base.VerifyHashedPassword(account, account.PasswordHash, password);
}
