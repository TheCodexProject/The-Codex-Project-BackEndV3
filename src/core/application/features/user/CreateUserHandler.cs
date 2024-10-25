using application.appEntry.commands.user;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.user;
using OperationResult;

namespace application.features.user;

public class CreateUserHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateUserCommand>
{
    public async Task<Result> HandleAsync(CreateUserCommand command)
    {
        // * Create the user
        var user = User.Create(command.FirstName, command.LastName, command.Email);

        // ? Was the user created successfully?
        if (user.IsFailure)
            // ! Return the errors
            return Result.Failure(user.Errors.ToArray());

        // * Save the user to the database
        await unitOfWork.Users.AddAsync(user.Value);

        // ? Did the save fail?
        if (await unitOfWork.SaveChangesAsync() == 0)
            return Result.Failure(new FailedOperationException("Failed to save the user to the database."));

        // * Set the user's ID to the command
        command.Id = user.Value.Id;

        // * Return the success result
        return Result.Success();
    }
}