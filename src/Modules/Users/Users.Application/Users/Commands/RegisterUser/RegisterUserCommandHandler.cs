using BuildingBlocks.Application;
using BuildingBlocks.Domain;
using Users.Contracts;
using Users.Domain;

namespace Users.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IUsersUnitOfWork unitOfWork,
    IEventBus eventBus) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        var firstNameResult = FirstName.Create(request.FirstName);
        if (firstNameResult.IsFailure)
        {
            return Result.Failure<Guid>(firstNameResult.Error);
        }

        var lastNameResult = LastName.Create(request.LastName);
        if (lastNameResult.IsFailure)
        {
            return Result.Failure<Guid>(lastNameResult.Error);
        }

        var emailExists = await userRepository.ExistsByEmailAsync(emailResult.Value, cancellationToken);
        if (emailExists)
        {
            return Result.Failure<Guid>(UserErrors.EmailAlreadyInUse);
        }

        var userResult = User.Create(
            firstNameResult.Value,
            lastNameResult.Value,
            emailResult.Value);

        if (userResult.IsFailure)
        {
            return Result.Failure<Guid>(userResult.Error);
        }

        var user = userResult.Value;
        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Publish cross-module integration event
        await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                user.Id.Value,
                user.Email.Value,
                user.FirstName.Value,
                user.LastName.Value),
            cancellationToken);

        return Result.Success(user.Id.Value);
    }
}
