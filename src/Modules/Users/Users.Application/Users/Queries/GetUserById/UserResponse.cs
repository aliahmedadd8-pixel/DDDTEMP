namespace Users.Application.Users.Queries.GetUserById;

public sealed record UserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    DateTime CreatedAtUtc);
