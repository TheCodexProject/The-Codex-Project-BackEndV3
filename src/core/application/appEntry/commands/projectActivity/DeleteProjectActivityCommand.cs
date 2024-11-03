using domain.exceptions;
using domain.models.projectActivity.value;
using OperationResult;

namespace application.appEntry.commands.projectActivity;

public class DeleteProjectActivityCommand
{
    // NOTE: Given information
    public Guid ActivityId { get; }
    public Guid ProjectId { get;}
    public ProjectActivityType Type { get; }

    private DeleteProjectActivityCommand(Guid activityId, Guid projectId, ProjectActivityType type)
    {
        ActivityId = activityId;
        ProjectId = projectId;
        Type = type;
    }

    public static Result<DeleteProjectActivityCommand> Create(string activityId, string projectId, ProjectActivityType type)
    {
        // ! Validate the input
        var validationResult = Validate(activityId, projectId);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<DeleteProjectActivityCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command
        return new DeleteProjectActivityCommand(new Guid(activityId),new Guid(projectId), type);
    }

    private static Result Validate(string activityId, string projectId)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = [];

        // ! Validate the ID
        if (!Guid.TryParse(activityId, out var _))
            exceptions.Add(new FailedOperationException("The given Activity ID could not be parsed into a GUID"));

        // ! Validate the owner ID (Guid)
        if (!Guid.TryParse(projectId, out var _))
            exceptions.Add(new FailedOperationException("The given Project ID could not be parsed into a GUID"));

        // ? Were there any exceptions?
        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray()) // ! Return errors.
            : Result.Success(); // * Return success.
    }

}