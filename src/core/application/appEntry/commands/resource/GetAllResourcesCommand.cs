using domain.models.resource;
using OperationResult;

namespace application.appEntry.commands.resource;

public class GetAllResourcesCommand
{
    public List<Resource> Resources { get; set; } = [];

    private GetAllResourcesCommand() { }

    public static Result<GetAllResourcesCommand> Create()
    {
        // ! No validation needed here.
        return new GetAllResourcesCommand();
    }
}