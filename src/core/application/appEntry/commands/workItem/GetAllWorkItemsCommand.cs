using domain.models.workItem;
using OperationResult;

namespace application.appEntry.commands.workItem;

public class GetAllWorkItemsCommand
{
    public List<WorkItem> WorkItems { get; set; } = [];

    private GetAllWorkItemsCommand() { }

    public static Result<GetAllWorkItemsCommand> Create()
    {
        return new GetAllWorkItemsCommand();
    }
}