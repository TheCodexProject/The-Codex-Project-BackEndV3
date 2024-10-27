using application.appEntry.commands.project;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.project;

public class GetAllProjectsHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetAllProjectsCommand>
{
    public async Task<Result> HandleAsync(GetAllProjectsCommand command)
    {
        // * Get all the projects
        var projects = await unitOfWork.Projects.GetAllAsync();

        // ? Were there any projects?
        var enumerable = projects.ToList();

        if (enumerable.Count == 0)
            // ! Return the error
            return Result.Failure(new NotFoundException("No projects were found in the database."));

        // * Return the projects
        command.Projects = enumerable;
        return Result.Success();
    }
    
}