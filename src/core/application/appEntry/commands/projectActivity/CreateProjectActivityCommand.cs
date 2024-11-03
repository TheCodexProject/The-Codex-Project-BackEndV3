using domain.exceptions;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using OperationResult;

namespace application.appEntry.commands.projectActivity;

public class CreateProjectActivityCommand
{
    // NOTE: Result information
    public Guid Id { get; set; }

    // NOTE: Given information
    public Guid ProjectId { get; }
    public string Title { get; }
    public ProjectActivityType Type { get; }

    private CreateProjectActivityCommand(Guid projectId, string title, ProjectActivityType type)
    {
        ProjectId = projectId;
        Title = title;
        Type = type;
    }

    public static Result<CreateProjectActivityCommand> Create(string projectId, string title, ProjectActivityType type)
    {
        // ! Validate the user's input
        var validationResult = Validate(projectId, title);

        // ? Were there any errors during the validation?
        if (validationResult.IsFailure)
            return Result<CreateProjectActivityCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command.
        return new CreateProjectActivityCommand(new Guid(projectId), title, type);
    }

    private static Result Validate(string projectId, string title)
    {
        // * Create a list to store any exceptions that occur during validation
        List<Exception> exceptions = [];

        // ! Try to parse the given project ID into a GUID
        if (!Guid.TryParse(projectId, out var parsedProjectId))
            exceptions.Add(new FailedOperationException("The given project ID could not be parsed into a GUID"));

        // ! Validate the title
        var titleValidation = ProjectActivityPropertyValidator.ValidateTitle(title);

        // ? Were there any errors during the title validation?
        if (titleValidation.IsFailure)
            exceptions.AddRange(titleValidation.Errors);

        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray()) // ! Return errors.
            : Result.Success(); // * Return success.
    }
}