using domain.exceptions;
using domain.models.workspace;
using OperationResult;

namespace application.appEntry.commands.workspace;

public class UpdateWorkspaceCommand
{
    public Guid Id { get; }

    public Workspace? Workspace { get; set; } = null;
    public string? Title { get; }
    public List<Guid>? ContactsToAdd { get; }
    public List<Guid>? ContactsToRemove { get; }
    public List<Guid>? ProjectsToAdd { get; }
    public List<Guid>? ProjectsToRemove { get; }

    private UpdateWorkspaceCommand(Guid id, string title, List<Guid> contactsToAdd, List<Guid> contactsToRemove, List<Guid> projectsToAdd, List<Guid> projectsToRemove)
    {
        Id = id;
        Title = title;
        ContactsToAdd = contactsToAdd ?? new List<Guid>();
        ContactsToRemove = contactsToRemove ?? new List<Guid>();
        ProjectsToAdd = projectsToAdd ?? new List<Guid>();
        ProjectsToRemove = projectsToRemove ?? new List<Guid>();
    }

    public static Result<UpdateWorkspaceCommand> Create(string id, string? title, List<string>? contactsToAdd, List<string>? contactsToRemove, List<string>? projectsToAdd, List<string>? projectsToRemove)
    {
        // ! Validate the user's input
        var validationResult = Validate(id, title, contactsToAdd, contactsToRemove, projectsToAdd, projectsToRemove);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<UpdateWorkspaceCommand>.Failure(validationResult.Errors.ToArray());

        // Convert to GUIDs, adding null checks to avoid NullReferenceException
        var contactsToAddGuids = contactsToAdd != null
            ? contactsToAdd.ConvertAll(Guid.Parse)
            : new List<Guid>();

        var contactsToRemoveGuids = contactsToRemove != null
            ? contactsToRemove.ConvertAll(Guid.Parse)
            : new List<Guid>();

        var projectsToAddGuids = projectsToAdd != null
            ? projectsToAdd.ConvertAll(Guid.Parse)
            : new List<Guid>();

        var projectsToRemoveGuids = projectsToRemove != null
            ? projectsToRemove.ConvertAll(Guid.Parse)
            : new List<Guid>();

        // * Return the newly created command.
        return new UpdateWorkspaceCommand(new Guid(id), title, contactsToAddGuids, contactsToRemoveGuids, projectsToAddGuids, projectsToRemoveGuids);
    }

    private static Result Validate(string id, string? title, List<string>? contactsToAdd, List<string>? contactsToRemove, List<string>? projectsToAdd, List<string>? projectsToRemove)
    {
        List<Exception> exceptions = [];

        // ! Validate the ID
        if (!Guid.TryParse(id, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        // ! Validate the title
        if (!string.IsNullOrWhiteSpace(title))
        {
            var titleValidation = WorkspacePropertyValidator.ValidateTitle(title);

            if (titleValidation.IsFailure)
                exceptions.AddRange(titleValidation.Errors);
        }

        // ! Validate the GUIDs to add
        if (contactsToAdd != null)
        {
            var contactsToAddValidation = ValidateIds(contactsToAdd);

            if (contactsToAddValidation.IsFailure)
                exceptions.AddRange(contactsToAddValidation.Errors);
        }

        // ! Validate the GUIDs to remove
        if (contactsToRemove != null)
        {
            var contactsToRemoveValidation = ValidateIds(contactsToRemove);

            if (contactsToRemoveValidation.IsFailure)
                exceptions.AddRange(contactsToRemoveValidation.Errors);
        }

        // ! Validate the GUIDs to add
        if (projectsToAdd != null)
        {
            var projectsToAddValidation = ValidateIds(projectsToAdd);

            if (projectsToAddValidation.IsFailure)
                exceptions.AddRange(projectsToAddValidation.Errors);
        }

        // ! Validate the GUIDs to remove
        if (projectsToRemove != null)
        {
            var projectsToRemoveValidation = ValidateIds(projectsToRemove);

            if (projectsToRemoveValidation.IsFailure)
                exceptions.AddRange(projectsToRemoveValidation.Errors);
        }

        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray())
            : Result.Success();
    }

    private static Result ValidateIds(List<string> members)
    {
        var failedMembers = new List<Exception>();

        foreach (var member in members)
        {
            if (!Guid.TryParse(member, out _))
                failedMembers.Add(new FailedOperationException($"The given member ID '{member}' could not be parsed into a GUID"));
        }

        return failedMembers.Count != 0
            ? Result.Failure(failedMembers.ToArray())
            : Result.Success();
    }
}