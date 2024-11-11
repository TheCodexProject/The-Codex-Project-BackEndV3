using domain.exceptions;
using domain.models.organization;
using OperationResult;

namespace application.appEntry.commands.organization;

public class CreateOrganizationCommand
{
    // NOTE: Given information
    public string Name { get; }
    public Guid OwnerId { get; }

    // NOTE: Result information
    public Organization? Organization { get; set; } = null;

    private CreateOrganizationCommand(string name, Guid ownerId)
    {
        Name = name;
        OwnerId = ownerId;
    }

    public static Result<CreateOrganizationCommand> Create(string name, string ownerId)
    {
        // ! Validate the user's input
        var validationResult = Validate(name, ownerId);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<CreateOrganizationCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command.
        return new CreateOrganizationCommand(name, new Guid(ownerId));
    }

    private static Result Validate(string name, string ownerId)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = [];

        // ! Validate the name
        var nameValidation = OrganizationPropertyValidator.ValidateName(name);

        // ? Did the name validation fail?
        if (nameValidation.IsFailure)
            exceptions.AddRange(nameValidation.Errors);

        // ! Validate the owner id
        if (!Guid.TryParse(ownerId, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        // ? Were there any exceptions?
        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray()) // * Yes: Return the exceptions
            : Result.Success(); // * No: Return success
    }

}