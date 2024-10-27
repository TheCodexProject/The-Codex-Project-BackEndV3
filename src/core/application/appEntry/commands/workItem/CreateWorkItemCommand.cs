using domain.exceptions;
using domain.models.workItem;
using OperationResult;

namespace application.appEntry.commands.workItem;

public class CreateWorkItemCommand
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public string Title { get; set; }

    private CreateWorkItemCommand(Guid projectId, string title)
    {
        ProjectId = projectId;
        Title = title;
    }

    public static Result<CreateWorkItemCommand> Create(string projectId, string title)
    {
        var validationResult = Validate(projectId, title);

        if (validationResult.IsFailure)
            return Result<CreateWorkItemCommand>.Failure(validationResult.Errors.ToArray());

        return new CreateWorkItemCommand(new Guid(projectId), title);
    }

    private static Result Validate(string workspaceId, string title)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = [];

        // ! Validate the project id
        if (!Guid.TryParse(workspaceId, out var parsedWorkspaceId))
            return Result.Failure(new FailedOperationException("The given workspace ID could not be parsed into a GUID"));

        // ! Validate the title
        var titleValidation = WorkItemPropertyValidator.ValidateTitle(title);

        // ? Did the title validation fail?
        if (titleValidation.IsFailure)
            exceptions.AddRange(titleValidation.Errors);

        // ? Were there any exceptions?
        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray()) // * Yes: Return the exceptions
            : Result.Success(); // * No: Return success
    }


}