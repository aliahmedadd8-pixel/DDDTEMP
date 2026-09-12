using BuildingBlocks.Domain;

namespace Users.Domain;

public static class UserErrors
{
    public static Error NotFound(UserId userId) =>
        Error.NotFound("User.NotFound", $"The user with ID '{userId.Value}' was not found.");

    public static readonly Error EmailAlreadyInUse =
        Error.Conflict("User.EmailAlreadyInUse", "The specified email is already in use.");
}
