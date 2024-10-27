using application.appEntry.commands.project;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.project;

public class DeleteProjectHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteProjectCommand>
{
    public async Task<Result> HandleAsync(DeleteProjectCommand command)
    {
        // ? Does the project exist?
        var project = await unitOfWork.Projects.GetByIdAsync(command.Id);

        // ? If the project does not exist
        if (project == null)
            return Result.Failure(new NotFoundException("The given project could not be found"));

        // * Delete the project
        unitOfWork.Projects.Remove(project);

        // ? Was the project deleted?
        if (await unitOfWork.SaveChangesAsync() == 0)
            return Result.Failure(new FailedOperationException("Project could not be deleted"));

        // * Return success
        return Result.Success();
    }
    
}