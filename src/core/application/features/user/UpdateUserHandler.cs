using application.appEntry.commands.user;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.user;
using OperationResult;

namespace application.features.user;

public class UpdateUserHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> HandleAsync(UpdateUserCommand command)
    {
        // ? Does the user exist?
        var user = await unitOfWork.Users.GetByIdAsync(command.Id);

        // ? If the user does not exist
        if (user == null)
            return Result.Failure(new NotFoundException("The given user could not be found"));


        // * Update the user
        if (IsChanged(user.FirstName, command.FirstName))
            user.UpdateFirstName(command.FirstName);

        if (IsChanged(user.LastName, command.LastName))
            user.UpdateLastName(command.LastName);

        if (IsChanged(user.Email, command.Email))
            user.UpdateEmail(command.Email);

        // * Save the user to the database
        await unitOfWork.SaveChangesAsync();

        // * Return success
        command.User = user;
        return Result.Success();
    }

    private static bool IsChanged(string? value, string? newValue)
    {
        return value != newValue && !string.IsNullOrWhiteSpace(newValue);
    }


}