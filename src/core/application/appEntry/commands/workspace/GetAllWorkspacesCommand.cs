using domain.models.workspace;
using OperationResult;

namespace application.appEntry.commands.workspace;

public class GetAllWorkspacesCommand
{
    public List<Workspace> Workspaces { get; set; } = [];

    private GetAllWorkspacesCommand() { }

    public static Result<GetAllWorkspacesCommand> Create()
    {
        return new GetAllWorkspacesCommand();
    }
    
}