namespace Users.Presentation.Contracts;

public sealed record RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName);
