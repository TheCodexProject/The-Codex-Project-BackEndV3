using domain.exceptions;
using domain.models.resource.values;
using OperationResult;

namespace application.appEntry.commands.resource;

public class DeleteResourceCommand
{
    public Guid Id { get; set; }
    public ResourceLevel Level { get; set; }
    public Guid OwnerId { get; set; }

    private DeleteResourceCommand(Guid id, Guid ownerId, ResourceLevel level)
    {
        Id = id;
        OwnerId = ownerId;
        Level = level;
    }
    
    public static Result<DeleteResourceCommand> Create(string id, string ownerId, ResourceLevel level)
    {
        // ! Validate the input
        var validationResult = Validate(id, ownerId);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<DeleteResourceCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command
        return new DeleteResourceCommand(new Guid(id),new Guid(ownerId), level);
    }

    private static Result Validate(string id, string ownerId)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = [];

        // ! Validate the ID
        if (!Guid.TryParse(id, out var parsedId))
            exceptions.Add(new FailedOperationException("The given ID could not be parsed into a GUID"));

        // ! Validate the owner ID (Guid)
        if (!Guid.TryParse(ownerId, out var parsedWorkspaceId))
            exceptions.Add(new FailedOperationException("The given Owner ID could not be parsed into a GUID"));

        // ? Were there any exceptions?
        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray()) // * Yes: Return the exceptions
            : Result.Success(); // * No: Return the parsed ID
    }
}