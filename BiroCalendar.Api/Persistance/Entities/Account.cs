namespace BiroCalendar.Api.Persistance.Entities;

public class Account
{
    public int Id { get; set; }

    public string EmailAddress { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public List<BiroAccount> BiroAccounts { get; set; } = null!;
}
