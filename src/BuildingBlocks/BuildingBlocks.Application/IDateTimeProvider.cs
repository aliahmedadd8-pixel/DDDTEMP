namespace BuildingBlocks.Application;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
