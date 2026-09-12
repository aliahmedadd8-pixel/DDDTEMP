namespace BuildingBlocks.Domain;

/// <summary>
/// Base record for strongly-typed identifiers to prevent primitive obsession.
/// </summary>
public abstract record StronglyTypedId<TValue>(TValue Value)
    where TValue : notnull
{
    public override string ToString() => Value.ToString()!;
}
