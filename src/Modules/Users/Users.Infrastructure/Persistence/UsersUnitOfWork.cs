using Users.Domain;

namespace Users.Infrastructure.Persistence;

public sealed class UsersUnitOfWork(UsersDbContext dbContext) : IUsersUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
