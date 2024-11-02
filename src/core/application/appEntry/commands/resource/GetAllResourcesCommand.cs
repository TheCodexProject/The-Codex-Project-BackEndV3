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

    private GetAllResourcesCommand() { }

    public static Result<GetAllResourcesCommand> Create()
    {
        // ! No validation needed here.
        return new GetAllResourcesCommand();
    }
}