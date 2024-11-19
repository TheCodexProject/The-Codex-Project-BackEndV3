using application.appEntry.commands.user;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.user;

public class GetAllUsersHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetAllUsersCommand>
{
    public async Task<Result> HandleAsync(GetAllUsersCommand command)
    {
        // * Get all the users
        var users = await unitOfWork.Users.GetAllAsync();

        // ? Were there any users?
        var enumerable = users.ToList();

        // * Return the users
        command.Users = enumerable;
        return Result.Success();
    }
}