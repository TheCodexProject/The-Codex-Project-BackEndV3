using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.workspace;

public class GetAllWorkspacesHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetAllWorkspacesCommand>
{
    public async Task<Result> HandleAsync(GetAllWorkspacesCommand command)
    {
        // * Get all the workspaces
        var workspaces = await unitOfWork.Workspaces.GetAllAsync();

        // ? Were there any workspaces?
        var enumerable = workspaces.ToList();

        if (enumerable.Count == 0)
            // ! Return the error
            return Result.Failure(new NotFoundException("No workspaces were found in the database."));

        // * Return the workspaces
        command.Workspaces = enumerable;
        return Result.Success();


    }
}