using domain.exceptions;
using domain.models.workItem;
using OperationResult;

namespace application.appEntry.commands.workItem;

public class GetProjectWorkItemsCommand
{
    // NOTE: Given information
    public Guid ProjectId { get; set; }

    // NOTE: Result information
    public List<WorkItem> WorkItems { get; set; } = [];

    private GetProjectWorkItemsCommand(Guid projectId)
    {
        ProjectId = projectId;
    }

    public static Result<GetProjectWorkItemsCommand> Create(string projectId)
    {
        // ! Validate the user's input
        var validationResult = Validate(projectId);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<GetProjectWorkItemsCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command.
        return new GetProjectWorkItemsCommand(new Guid(projectId));
    }

    private static Result Validate(string projectId)
    {
        // ! Validate the ID
        return !Guid.TryParse(projectId, out var _)
            ? Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"))
            : Result.Success(); // * No: Return success
    }





}