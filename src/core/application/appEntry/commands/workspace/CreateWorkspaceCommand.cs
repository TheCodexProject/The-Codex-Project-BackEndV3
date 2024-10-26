using domain.exceptions;
using domain.models.workspace;
using OperationResult;

namespace application.appEntry.commands.workspace;

public class CreateWorkspaceCommand
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; }
    public string Title { get;  }

    private CreateWorkspaceCommand(Guid organizationId, string title)
    {
        OrganizationId = organizationId;
        Title = title;
    }

    public static Result<CreateWorkspaceCommand> Create(string organizationId, string title)
    {
        // ! Validate the user's input
        var validationResult = Validate(organizationId, title);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<CreateWorkspaceCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command.
        return new CreateWorkspaceCommand(new Guid(organizationId), title);
    }

    private static Result Validate(string organizationId, string title)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = [];

        // ! Validate the organization id
        if (!Guid.TryParse(organizationId, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        // ! Validate the title
        var titleValidation = WorkspacePropertyValidator.ValidateTitle(title);

        // ? Did the title validation fail?
        if (titleValidation.IsFailure)
            exceptions.AddRange(titleValidation.Errors);

        // ? Were there any exceptions?
        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray()) // * Yes: Return the exceptions
            : Result.Success(); // * No: Return success
    }





}