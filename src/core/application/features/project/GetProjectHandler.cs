using application.appEntry.commands.project;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.project;

public class GetProjectHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetProjectCommand>
{
    public async Task<Result> HandleAsync(GetProjectCommand command)
    {
        // * Get the project from the database
        var project = await unitOfWork.Projects.GetByIdAsync(command.Id);

        // ? Was the project found?
        if (project is null)
            // ! Return the errors
            return Result.Failure(new NotFoundException("The project was not found."));

        // * Return the project
        command.Project = project;
        return Result.Success();
    }
    
}