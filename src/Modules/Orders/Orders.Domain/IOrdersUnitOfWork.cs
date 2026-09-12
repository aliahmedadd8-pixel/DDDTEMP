namespace Orders.Domain;

public interface IOrdersUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
