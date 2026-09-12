using BuildingBlocks.Application;

namespace Users.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName) : ICommand<Guid>;
