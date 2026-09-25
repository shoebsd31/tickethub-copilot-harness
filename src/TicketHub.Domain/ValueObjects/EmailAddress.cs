using TicketHub.Domain.Common;
using TicketHub.Domain.Errors;

namespace TicketHub.Domain.ValueObjects;

/// <summary>A normalized customer email address. Can never exist in an invalid state.</summary>
public sealed record EmailAddress
{
    private EmailAddress(string value) => Value = value;

    /// <summary>The normalized (trimmed, lower-case) address.</summary>
    public string Value { get; }

    /// <summary>Validates and normalizes an email address.</summary>
    public static Result<EmailAddress> Create(string? email)
    {
        var candidate = email?.Trim().ToLowerInvariant() ?? string.Empty;
        var at = candidate.IndexOf('@');
        var isValid = at > 0 && at < candidate.Length - 1 && !candidate.Contains(' ');

        return isValid
            ? Result<EmailAddress>.Success(new EmailAddress(candidate))
            : Result<EmailAddress>.Failure(DomainErrors.Email.InvalidFormat);
    }

    /// <inheritdoc />
    public override string ToString() => Value;
}
