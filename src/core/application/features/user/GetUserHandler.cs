using application.appEntry.commands.user;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.user;

public class GetUserHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetUserCommand>
{
    public async Task<Result> HandleAsync(GetUserCommand command)
    {
        // * Get the user from the database
        var user = await unitOfWork.Users.GetByIdAsync(command.Id);

        // ? Was the user found?
        if (user is null)
            // ! Return the errors
            return Result.Failure(new NotFoundException("The user was not found."));

        // * Return the user
        command.User = user;
        return Result.Success();
    }
}