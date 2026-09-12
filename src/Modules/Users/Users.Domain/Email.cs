using System.Text.RegularExpressions;
using BuildingBlocks.Domain;

namespace Users.Domain;

public sealed partial class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(Error.Validation("Email.Empty", "Email cannot be empty."));
        }

        var trimmed = email.Trim().ToLowerInvariant();

        if (trimmed.Length > 255)
        {
            return Result.Failure<Email>(Error.Validation("Email.TooLong", "Email cannot exceed 255 characters."));
        }

        if (!EmailRegex.IsMatch(trimmed))
        {
            return Result.Failure<Email>(Error.Validation("Email.InvalidFormat", "Email format is invalid."));
        }

        return Result.Success(new Email(trimmed));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
