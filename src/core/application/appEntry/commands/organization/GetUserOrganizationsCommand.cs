using application.appEntry.commands.workItem;
using domain.exceptions;
using domain.models.organization;
using OperationResult;

namespace application.appEntry.commands.organization;

public class GetUserOrganizationsCommand
{
    // NOTE: Given information
    public Guid UserId { get; set; }

    // NOTE: Result information
    public List<Organization> Organizations { get; set; } = [];

    private GetUserOrganizationsCommand(Guid userId)
    {
        UserId = userId;
    }

    public static Result<GetUserOrganizationsCommand> Create(string userId)
    {
        // ! Validate the user's input
        var validationResult = Validate(userId);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<GetUserOrganizationsCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command.
        return new GetUserOrganizationsCommand(new Guid(userId));
    }

    private static Result Validate(string userId)
    {
        // ! Validate the ID
        return !Guid.TryParse(userId, out var _)
            ? Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"))
            : Result.Success(); // * No: Return success
    }
}