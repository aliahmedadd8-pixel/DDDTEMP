using Microsoft.EntityFrameworkCore;
using Orders.Domain;
using Orders.Infrastructure.Persistence;

namespace Orders.Infrastructure.Repositories;

public sealed class CustomerRepository(OrdersDbContext dbContext) : ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(CustomerId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Customers.AnyAsync(c => c.Id == id, cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await dbContext.Customers.AddAsync(customer, cancellationToken);
    }
}
