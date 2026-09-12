namespace BuildingBlocks.Domain;

/// <summary>
/// Base class for Aggregate Roots in DDD.
/// Aggregate Roots enforce transactional consistency boundaries and encapsulate business logic.
/// </summary>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    protected AggregateRoot(TId id) : base(id)
    {
    }

    // Required for EF Core
    protected AggregateRoot()
    {
    }
}
