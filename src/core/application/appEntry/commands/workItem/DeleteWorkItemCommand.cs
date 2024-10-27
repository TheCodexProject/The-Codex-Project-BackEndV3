using domain.exceptions;
using OperationResult;

namespace application.appEntry.commands.workItem;

public class DeleteWorkItemCommand
{
    public Guid Id { get; set; }

    private DeleteWorkItemCommand(Guid id)
    {
        Id = id;
    }

    public static Result<DeleteWorkItemCommand> Create(string id)
    {
        // ! Validate the user's input
        var validationResult = Validate(id);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<DeleteWorkItemCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command.
        return new DeleteWorkItemCommand(new Guid(id));
    }

    private static Result Validate(string id)
    {
        // ! Validate the ID
        if (!Guid.TryParse(id, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        // ? Were there any exceptions?
        return Result.Success(); // * No: Return success
    }
}