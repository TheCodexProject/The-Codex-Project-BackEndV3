using application.appEntry.commands.user;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.user;

public class DeleteUserHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> HandleAsync(DeleteUserCommand command)
    {
        // ? Does the user exist?
        var user = await unitOfWork.Users.GetByIdAsync(command.Id);

        // ? If the user does not exist
        if (user == null)
            return Result.Failure(new NotFoundException("The given user could not be found"));

        // * Delete the user
        unitOfWork.Users.Remove(user);

        // ? Was the user deleted?
        if (await unitOfWork.SaveChangesAsync() == 0)
            return Result.Failure(new FailedOperationException("User could not be deleted"));

        // * Return success
        return Result.Success();
    }
}