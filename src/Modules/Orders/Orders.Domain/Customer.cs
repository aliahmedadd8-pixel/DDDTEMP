using BuildingBlocks.Domain;

namespace Orders.Domain;

public sealed class Customer : Entity<CustomerId>
{
    private Customer(CustomerId id, string email, string fullName) : base(id)
    {
        Email = email;
        FullName = fullName;
    }

    // Required for EF Core
    private Customer()
    {
    }

    public string Email { get; private set; } = default!;
    public string FullName { get; private set; } = default!;

    public static Customer Create(CustomerId id, string email, string fullName)
    {
        return new Customer(id, email, fullName);
    }
}
