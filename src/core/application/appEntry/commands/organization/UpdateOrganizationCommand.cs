using domain.exceptions;
using domain.models.organization;
using domain.models.user;
using OperationResult;

namespace application.appEntry.commands.organization;

public class UpdateOrganizationCommand
{
    public Guid Id { get; }

    public Organization? Organization { get; set; } = null;
    public string? Name { get; }
    public List<Guid> MembersToAdd { get; }
    public List<Guid> MembersToRemove { get; }

    private UpdateOrganizationCommand(Guid id, string? name, List<Guid> membersToAdd, List<Guid> membersToRemove)
    {
        Id = id;
        Name = name;
        MembersToAdd = membersToAdd ?? new List<Guid>();
        MembersToRemove = membersToRemove ?? new List<Guid>();
    }

    public static Result<UpdateOrganizationCommand> Create(string id, string? name, List<string>? membersToAdd, List<string>? membersToRemove)
    {
        // ! Validate the user's input
        var validationResult = Validate(id, name, membersToAdd, membersToRemove);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<UpdateOrganizationCommand>.Failure(validationResult.Errors.ToArray());

        // Convert to GUIDs, adding null checks to avoid NullReferenceException
        var membersToAddGuids = membersToAdd != null
            ? membersToAdd.ConvertAll(Guid.Parse)
            : new List<Guid>();

        var membersToRemoveGuids = membersToRemove != null
            ? membersToRemove.ConvertAll(Guid.Parse)
            : new List<Guid>();

        // * Return the newly created command.
        return new UpdateOrganizationCommand(new Guid(id), name, membersToAddGuids, membersToRemoveGuids);
    }

    private static Result Validate(string id, string? name, List<string>? membersToAdd, List<string>? membersToRemove)
    {
        // ! Validate the id
        if (!Guid.TryParse(id, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        // ! Validate the name
        if (!string.IsNullOrWhiteSpace(name))
        {
            var nameValidationResult = OrganizationPropertyValidator.ValidateName(name);

            if (nameValidationResult.IsFailure)
                return Result.Failure(nameValidationResult.Errors.ToArray());
        }

        // ! Validate the member IDs to add
        if (membersToAdd != null)
        {
            var membersToAddValidationResult = ValidateMembers(membersToAdd);

            if (membersToAddValidationResult.IsFailure)
                return Result.Failure(membersToAddValidationResult.Errors.ToArray());
        }

        // ! Validate the member IDs to remove
        if (membersToRemove != null)
        {
            var membersToRemoveValidationResult = ValidateMembers(membersToRemove);

            if (membersToRemoveValidationResult.IsFailure)
                return Result.Failure(membersToRemoveValidationResult.Errors.ToArray());
        }

        return Result.Success();
    }

    private static Result ValidateMembers(List<string> members)
    {
        // * List of failed member IDs
        var failedMembers = new List<Exception>();

        // Check that all the member IDs are valid
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