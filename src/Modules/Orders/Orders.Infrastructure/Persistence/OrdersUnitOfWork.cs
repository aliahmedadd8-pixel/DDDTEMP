using Orders.Domain;

namespace Orders.Infrastructure.Persistence;

public sealed class OrdersUnitOfWork(OrdersDbContext dbContext) : IOrdersUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
