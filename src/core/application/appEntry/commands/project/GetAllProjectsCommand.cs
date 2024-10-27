using domain.models.project;
using OperationResult;

namespace application.appEntry.commands.project;

public class GetAllProjectsCommand
{
    public List<Project> Projects { get; set; } = [];

    private GetAllProjectsCommand() { }

    public static Result<GetAllProjectsCommand> Create()
    {
        return new GetAllProjectsCommand();
    }
}