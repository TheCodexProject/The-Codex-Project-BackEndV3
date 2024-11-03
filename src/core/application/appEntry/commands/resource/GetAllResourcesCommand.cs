using domain.exceptions;
using domain.models.resource;
using domain.models.resource.values;
using OperationResult;

namespace application.appEntry.commands.resource;

public class GetAllResourcesCommand
{
    // NOTE: Given information
    public Guid OwnerId { get; set; }
    public ResourceLevel Level { get; set; }

    // NOTE: Result information
    public List<Resource> Resources { get; set; } = [];

    private GetAllResourcesCommand(Guid ownerId, ResourceLevel level)
    {
        OwnerId = ownerId;
        Level = level;
    }

    public static Result<GetAllResourcesCommand> Create(string ownerId, ResourceLevel level)
    {
        // ? Is the ID valid?
        if (!Guid.TryParse(ownerId, out var parsedWorkspaceId))
            return Result<GetAllResourcesCommand>.Failure(new FailedOperationException("The given Owner ID could not be parsed into a GUID"));

        return new GetAllResourcesCommand(new Guid(ownerId), level);
    }
}