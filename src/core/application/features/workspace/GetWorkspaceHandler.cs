using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.workspace;

public class GetWorkspaceHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetWorkspaceCommand>
{
    public async Task<Result> HandleAsync(GetWorkspaceCommand command)
    {
        // * Get the workspace from the database
        var workspace = await unitOfWork.Workspaces.GetByIdAsync(command.Id);

        // ? Was the workspace found?
        if (workspace is null)
            // ! Return the errors
            return Result.Failure(new NotFoundException("The workspace was not found."));

        // * Return the workspace
        command.Workspace = workspace;
        return Result.Success();
    }
}