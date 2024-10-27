using domain.exceptions;
using domain.models.project;
using OperationResult;

namespace application.appEntry.commands.project;

public class CreateProjectCommand
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }
    public string Title { get; set; }

    private CreateProjectCommand(Guid workspaceId, string title)
    {
        WorkspaceId = workspaceId;
        Title = title;
    }

    public static Result<CreateProjectCommand> Create(string workspaceId, string title)
    {
        var validationResult = Validate(workspaceId, title);

        if (validationResult.IsFailure)
            return Result<CreateProjectCommand>.Failure(validationResult.Errors.ToArray());

        return new CreateProjectCommand(new Guid(workspaceId), title);
    }

    private static Result Validate( string workspaceId, string title)
    {
        List<Exception> exceptions = [];

        if (!Guid.TryParse(workspaceId, out var parsedWorkspaceId))
            return Result.Failure(new FailedOperationException("The given workspace ID could not be parsed into a GUID"));

        var titleValidation = ProjectPropertyValidator.ValidateTitle(title);

        if (titleValidation.IsFailure)
            exceptions.AddRange(titleValidation.Errors);

        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray())
            : Result.Success();
    }

}