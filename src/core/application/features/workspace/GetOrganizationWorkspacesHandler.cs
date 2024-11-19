using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using domain.interfaces;
using OperationResult;

namespace application.features.workspace;

public class GetOrganizationWorkspacesHandler(IUnitOfWork unitOfWork): ICommandHandler<GetOrganizationWorkspacesCommand>
{
    public async Task<Result> HandleAsync(GetOrganizationWorkspacesCommand command)
    {
        // * Get all workspaces
        var workspaces = await unitOfWork.Workspaces.GetAllAsync();

        // ? Were there any workspaces?
        var enumerable = workspaces.ToList();

        // * Filter the workspaces by organization ID
        var organizationWorkspaces = enumerable.Where(w => w.Owner.Id == command.OrganizationId).ToList();

        // * Return success
        command.Workspaces = organizationWorkspaces;
        return Result.Success();
    }
}