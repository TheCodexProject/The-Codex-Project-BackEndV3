using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.workspace;

public class DeleteWorkspaceHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteWorkspaceCommand>
{
    public async Task<Result> HandleAsync(DeleteWorkspaceCommand command)
    {
        // ? Does the workspace exist?
        var workspace = await unitOfWork.Workspaces.GetByIdAsync(command.Id);

        // ? If the workspace does not exist
        if (workspace == null)
            return Result.Failure(new NotFoundException("The given workspace could not be found"));

        // * Delete the workspace
        unitOfWork.Workspaces.Remove(workspace);

        // ? Was the workspace deleted?
        if (await unitOfWork.SaveChangesAsync() == 0)
            return Result.Failure(new FailedOperationException("Workspace could not be deleted"));

        // * Return success
        return Result.Success();
    }
}