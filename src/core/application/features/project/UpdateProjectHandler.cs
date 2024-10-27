using application.appEntry.commands.project;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.shared;
using OperationResult;

namespace application.features.project;

public class UpdateProjectHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateProjectCommand>
{
    public async Task<Result> HandleAsync(UpdateProjectCommand command)
    {
        // * Get the project from the database
        var project = await unitOfWork.Projects.GetByIdAsync(command.Id);

        // ? Was the project found?
        if (project is null)
            // ! Return the errors
            return Result.Failure(new NotFoundException("The project was not found."));

        // * Update the project
        if(IsChanged(project.Title, command.Title))
            project.UpdateTitle(command.Title);

        if(IsChanged(project.Description, command.Description))
            project.UpdateDescription(command.Description);

        if(command.ProjectStatus.HasValue && (command.ProjectStatus != project.Status || command.ProjectStatus != Status.None))
            project.UpdateStatus(command.ProjectStatus.Value);

        if(command.ProjectPriority.HasValue && (project.Priority != command.ProjectPriority || command.ProjectPriority != Priority.None))
            project.UpdatePriority(command.ProjectPriority.Value);

        if((command.Start.HasValue && project.Start != command.Start) && (command.End.HasValue && project.End != command.End))
            project.UpdateTimeRange(command.Start.Value, command.End.Value);

        // * Save the changes
        await unitOfWork.SaveChangesAsync();

        // * Return the result
        command.Project = project;
        return Result.Success();
    }


    private static bool IsChanged(string? value, string? newValue)
    {
        return value != newValue && !string.IsNullOrWhiteSpace(newValue);
    }
}