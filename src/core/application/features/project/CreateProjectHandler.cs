using application.appEntry.commands.project;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.project;
using OperationResult;

namespace application.features.project;

public class CreateProjectHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateProjectCommand>
{
    public async Task<Result> HandleAsync(CreateProjectCommand command)
    {
        // * First find the workspace
        var workspace = await unitOfWork.Workspaces.GetByIdAsync(command.WorkspaceId);

        // ? If the workspace does not exist
        if (workspace == null)
            return Result.Failure(new NotFoundException("The given workspace could not be found"));

        // * Create the project
        var project = Project.Create(workspace, command.Title);

        // ? Were there any validation errors?
        if (project.IsFailure)
            return Result.Failure(project.Errors.ToArray());

        // * Save the project to the database
        await unitOfWork.Projects.AddAsync(project.Value);

        // ? Did the save fail?
        if (await unitOfWork.SaveChangesAsync() == 0)
            return Result.Failure(new FailedOperationException("Failed to save the project to the database."));

        // * Set the project's ID to the command
        command.Id = project.Value.Id;

        // * Return the success result
        return Result.Success();
    }
    
}