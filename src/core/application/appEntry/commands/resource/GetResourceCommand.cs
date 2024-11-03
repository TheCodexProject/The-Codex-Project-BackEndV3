using domain.exceptions;
using domain.models.resource;
using domain.models.resource.values;
using OperationResult;

namespace application.appEntry.commands.resource;

public class GetResourceCommand
{
    // NOTE: Given information
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public ResourceLevel Level { get; set; }

    // NOTE: Result information
    public Resource? Resource { get; set; } = null;


    private GetResourceCommand(Guid id, Guid ownerId, ResourceLevel level)
    {
        Id = id;
        OwnerId = ownerId;
        Level = level;
    }

    public static Result<GetResourceCommand> Create(string id, string ownerId, ResourceLevel level)
    {
        var validationResult = Validate(id, ownerId);

        if (validationResult.IsFailure)
            return Result<GetResourceCommand>.Failure(validationResult.Errors.ToArray());

        return new GetResourceCommand(Guid.Parse(id), Guid.Parse(ownerId), level);
    }

    private static Result Validate(string id, string ownerId)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        if (!Guid.TryParse(ownerId, out var parsedOwnerId))
            return Result.Failure(new FailedOperationException("The given owner ID could not be parsed into a GUID"));

        return Result.Success();
    }
}