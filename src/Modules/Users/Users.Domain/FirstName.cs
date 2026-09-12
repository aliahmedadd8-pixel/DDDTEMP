using BuildingBlocks.Domain;

namespace Users.Domain;

public sealed class FirstName : ValueObject
{
    private FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<FirstName> Create(string? firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<FirstName>(Error.Validation("FirstName.Empty", "First name cannot be empty."));
        }

        var trimmed = firstName.Trim();
        if (trimmed.Length > 100)
        {
            return Result.Failure<FirstName>(Error.Validation("FirstName.TooLong", "First name cannot exceed 100 characters."));
        }

        return Result.Success(new FirstName(trimmed));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
