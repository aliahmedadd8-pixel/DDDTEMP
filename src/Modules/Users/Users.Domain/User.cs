using BuildingBlocks.Domain;

namespace Users.Domain;

public sealed class User : AggregateRoot<UserId>, IAuditableEntity, ISoftDeletable
{
    private User(
        UserId id,
        FirstName firstName,
        LastName lastName,
        Email email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        IsActive = true;
    }

    // Required for EF Core
    private User()
    {
    }

    public FirstName FirstName { get; private set; } = default!;
    public LastName LastName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public bool IsActive { get; private set; }

    // IAuditableEntity
    public DateTime CreatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedAtUtc { get; set; }
    public string? LastModifiedBy { get; set; }

    // ISoftDeletable
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }

    public static Result<User> Create(
        FirstName firstName,
        LastName lastName,
        Email email)
    {
        var user = new User(
            UserId.New(),
            firstName,
            lastName,
            email);

        user.AddDomainEvent(new UserRegisteredDomainEvent(user.Id, user.Email));

        return Result.Success(user);
    }

    public Result UpdateName(FirstName firstName, LastName lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        return Result.Success();
    }

    public void Deactivate()
    {
        IsActive = false;
        IsDeleted = true;
        DeletedAtUtc = DateTime.UtcNow;
    }
}
