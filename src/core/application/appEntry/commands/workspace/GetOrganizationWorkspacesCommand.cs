using domain.exceptions;
using domain.models.workItem;
using domain.models.workspace;
using OperationResult;

namespace application.appEntry.commands.workspace;

public class GetOrganizationWorkspacesCommand
{
    // NOTE: Given information
    public Guid OrganizationId { get; set; }

    // NOTE: Result information
    public List<Workspace> Workspaces { get; set; } = [];

    private GetOrganizationWorkspacesCommand(Guid organizationId)
    {
        OrganizationId = organizationId;
    }

    public static Result<GetOrganizationWorkspacesCommand> Create(string organizationId)
    {
        // ! Validate the user's input
        var validationResult = Validate(organizationId);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<GetOrganizationWorkspacesCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command.
        return new GetOrganizationWorkspacesCommand(new Guid(organizationId));
    }

    private static Result Validate(string organizationId)
    {
        // ! Validate the ID
        return !Guid.TryParse(organizationId, out var _)
            ? Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"))
            : Result.Success(); // * No: Return success
    }
}