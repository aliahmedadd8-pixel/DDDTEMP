using BuildingBlocks.Domain;

namespace Users.Domain;

public sealed class LastName : ValueObject
{
    private LastName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<LastName> Create(string? lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<LastName>(Error.Validation("LastName.Empty", "Last name cannot be empty."));
        }

        var trimmed = lastName.Trim();
        if (trimmed.Length > 100)
        {
            return Result.Failure<LastName>(Error.Validation("LastName.TooLong", "Last name cannot exceed 100 characters."));
        }

        return Result.Success(new LastName(trimmed));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
