using application.appEntry.commands.project;
using application.appEntry.interfaces;
using domain.interfaces;
using OperationResult;

namespace application.features.project;

public class GetWorkspaceProjectsHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetWorkspaceProjectsCommand>
{
    public async Task<Result> HandleAsync(GetWorkspaceProjectsCommand command)
    {
        // * Get all the projects
        var projects = await unitOfWork.Projects.GetAllAsync();

        // ? Were there any projects?
        var enumerable = projects.ToList();

        // * Filter the projects by workspace ID
        var workspaceProjects = enumerable.Where(p => p.Workspace.Id == command.WorkspaceId).ToList();

        // * Return the projects
        command.Projects = workspaceProjects;
        return Result.Success();
    }
}