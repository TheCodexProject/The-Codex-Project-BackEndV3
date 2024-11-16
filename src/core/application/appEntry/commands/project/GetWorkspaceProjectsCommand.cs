using domain.exceptions;
using domain.models.project;
using domain.models.workItem;
using OperationResult;

namespace application.appEntry.commands.project;

public class GetWorkspaceProjectsCommand
{
    // NOTE: Given information
    public Guid WorkspaceId { get; set; }

    // NOTE: Result information
    public List<Project> Projects { get; set; } = [];

    private GetWorkspaceProjectsCommand(Guid workspaceId)
    {
        WorkspaceId = workspaceId;
    }

    public static Result<GetWorkspaceProjectsCommand> Create(string workspaceId)
    {
        // ! Validate the user's input
        var validationResult = Validate(workspaceId);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<GetWorkspaceProjectsCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command.
        return new GetWorkspaceProjectsCommand(new Guid(workspaceId));
    }

    private static Result Validate(string workspaceId)
    {
        // ! Validate the ID
        return !Guid.TryParse(workspaceId, out var _)
            ? Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"))
            : Result.Success(); // * No: Return success
    }
}