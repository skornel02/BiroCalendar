using System.Diagnostics.CodeAnalysis;

namespace BiroCalendar.Shared.Dtos;

public record BiroAccountCreationDto
{
    public string? AccountName { get; set; } = default!;
    public string? AccountPassword { get; set; } = default!;
    public string? ServiceUrl { get; set; } = default!;

    [MemberNotNullWhen(true, nameof(AccountName))]
    [MemberNotNullWhen(true, nameof(AccountPassword))]
    [MemberNotNullWhen(true, nameof(ServiceUrl))]
    public bool IsValidCreation => !string.IsNullOrEmpty(AccountName)
        && !string.IsNullOrEmpty(AccountPassword)
        && !string.IsNullOrEmpty(ServiceUrl);

    public bool IsValidUpdate => !string.IsNullOrEmpty(AccountName) 
        || !string.IsNullOrEmpty(AccountPassword)
        || !string.IsNullOrEmpty(ServiceUrl);
}
