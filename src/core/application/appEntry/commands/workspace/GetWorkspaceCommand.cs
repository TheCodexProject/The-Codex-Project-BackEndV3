using domain.exceptions;
using domain.models.workspace;
using OperationResult;

namespace application.appEntry.commands.workspace;

public class GetWorkspaceCommand
{
    public Guid Id { get; set; }

    public Workspace? Workspace { get; set; } = null;

    private GetWorkspaceCommand(Guid id)
    {
        Id = id;
    }

    public static Result<GetWorkspaceCommand> Create(string id)
    {
        var validationResult = Validate(id);

        if (validationResult.IsFailure)
            return Result<GetWorkspaceCommand>.Failure(validationResult.Errors.ToArray());

        return new GetWorkspaceCommand(new Guid(id));
    }

    private static Result Validate(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        return Result.Success();
    }
}