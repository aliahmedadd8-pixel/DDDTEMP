using BuildingBlocks.Domain;

namespace Orders.Domain;

public sealed class Money : ValueObject
{
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }
    public string Currency { get; }

    public static Result<Money> Create(decimal amount, string? currency = "USD")
    {
        if (amount < 0)
        {
            return Result.Failure<Money>(Error.Validation("Money.Negative", "Money amount cannot be negative."));
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            return Result.Failure<Money>(Error.Validation("Money.CurrencyEmpty", "Currency code cannot be empty."));
        }

        var normalizedCurrency = currency.Trim().ToUpperInvariant();
        if (normalizedCurrency.Length != 3)
        {
            return Result.Failure<Money>(Error.Validation("Money.InvalidCurrency", "Currency code must be 3 characters (e.g. USD, EUR, SAR)."));
        }

        return Result.Success(new Money(amount, normalizedCurrency));
    }

    public static Money Zero(string currency = "USD") => new(0m, currency.Trim().ToUpperInvariant());

    public Result<Money> Add(Money other)
    {
        if (Currency != other.Currency)
        {
            return Result.Failure<Money>(Error.Validation("Money.CurrencyMismatch", $"Cannot add money with different currencies '{Currency}' and '{other.Currency}'."));
        }

        return Create(Amount + other.Amount, Currency);
    }

    public Result<Money> Multiply(int multiplier)
    {
        if (multiplier < 0)
        {
            return Result.Failure<Money>(Error.Validation("Money.NegativeMultiplier", "Multiplier cannot be negative."));
        }

        return Create(Amount * multiplier, Currency);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:F2} {Currency}";
}
