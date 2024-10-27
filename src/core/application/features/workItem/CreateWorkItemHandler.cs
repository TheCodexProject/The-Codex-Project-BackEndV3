using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.workItem;
using OperationResult;

namespace application.features.workItem;

public class CreateWorkItemHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateWorkItemCommand>
{
    public async Task<Result> HandleAsync(CreateWorkItemCommand command)
    {
        // * First find the project
        var project = await unitOfWork.Projects.GetByIdAsync(command.ProjectId);

        // ? Was the project found?
        if (project is null)
            return Result.Failure(new NotFoundException("The provided project was not found, please provide a valid project."));

        // * Create the work item
        var workItem = WorkItem.Create(project, command.Title);

        // ? Were there any validation errors?
        if (workItem.IsFailure)
            return Result.Failure(workItem.Errors.ToArray());

        // * Save the work item to the database
        await unitOfWork.WorkItems.AddAsync(workItem.Value);

        // ? Did the save fail?
        if (await unitOfWork.SaveChangesAsync() == 0)
            return Result.Failure(new FailedOperationException("Failed to save the work item to the database."));

        // * Set the work item's ID to the command
        command.Id = workItem.Value.Id;

        // * Return the success result
        return Result.Success();
    }
}