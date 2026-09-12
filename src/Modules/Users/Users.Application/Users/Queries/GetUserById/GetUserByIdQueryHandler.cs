using BuildingBlocks.Application;
using BuildingBlocks.Domain;
using Users.Domain;

namespace Users.Application.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.From(request.UserId);
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(userId));
        }

        var response = new UserResponse(
            user.Id.Value,
            user.Email.Value,
            user.FirstName.Value,
            user.LastName.Value,
            user.IsActive,
            user.CreatedAtUtc);

        return Result.Success(response);
    }
}
