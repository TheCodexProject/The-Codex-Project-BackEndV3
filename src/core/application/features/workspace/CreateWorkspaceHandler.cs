using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.workspace;
using OperationResult;

namespace application.features.workspace;

public class CreateWorkspaceHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateWorkspaceCommand>
{
    public async Task<Result> HandleAsync(CreateWorkspaceCommand command)
    {
        // * First find the owner
        var owner = await unitOfWork.Organizations.GetByIdAsync(command.OrganizationId);

        // ? Was the owner found?
        if (owner is null)
            return Result.Failure(new NotFoundException("The provided owner was not found, please provide a valid owner."));

        // * Create the workspace
        var workspace = Workspace.Create(owner, command.Title);

        // ? Were there any validation errors?
        if (workspace.IsFailure)
            return Result.Failure(workspace.Errors.ToArray());

        // * Save the workspace to the database
        await unitOfWork.Workspaces.AddAsync(workspace.Value);

        // ? Did the save fail?
        if (await unitOfWork.SaveChangesAsync() == 0)
            return Result.Failure(new FailedOperationException("Failed to save the workspace to the database."));

        // * Set the workspace's ID to the command
        command.Id = workspace.Value.Id;

        // * Return the success result
        return Result.Success();
    }
}