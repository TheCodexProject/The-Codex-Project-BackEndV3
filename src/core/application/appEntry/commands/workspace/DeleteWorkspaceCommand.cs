using domain.exceptions;
using OperationResult;

namespace application.appEntry.commands.workspace;

public class DeleteWorkspaceCommand
{
    public Guid Id { get; set; }

    private DeleteWorkspaceCommand(Guid id)
    {
        Id = id;
    }

    public static Result<DeleteWorkspaceCommand> Create(string id)
    {
        // ! Validate the user's input
        var validationResult = Validate(id);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<DeleteWorkspaceCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command.
        return new DeleteWorkspaceCommand(new Guid(id));
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