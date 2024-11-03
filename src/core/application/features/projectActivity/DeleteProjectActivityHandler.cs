using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using OperationResult;

namespace application.features.projectActivity;

public class DeleteProjectActivityHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteProjectActivityCommand>
{
    public async Task<Result> HandleAsync(DeleteProjectActivityCommand command)
    {
        // * First find the project
        var project = await unitOfWork.Projects.GetByIdAsync(command.ProjectId);

        // ? Was the project found?
        if (project is null)
            // ! Return the error
            return Result.Failure(new NotFoundException("The project could not be found."));

        // * Find the activity in the project
        ProjectActivity? activity = null!;
        switch (command.Type)
        {
            case ProjectActivityType.Milestone:
                activity = project.Milestones.FirstOrDefault(m => m.Id == command.ActivityId);
                break;
            case ProjectActivityType.Iteration:
                activity = project.Iterations.FirstOrDefault(i => i.Id == command.ActivityId);
                break;
        }

        // ? Was the activity found?
        if (activity is null)
            // ! Return the error
            return Result.Failure(new NotFoundException("The activity could not be found."));

        // * Remove the activity from the project
        var removeActivityResult = project.RemoveActivity(activity);

        // ? Were there any validation errors?
        if (removeActivityResult.IsFailure)
            // ! Return the error
            return Result.Failure(removeActivityResult.Errors.ToArray());

        // * Remove the activity from the database
        unitOfWork.ProjectActivities.Remove(activity);

        // ? Did the save fail?
        if (await unitOfWork.SaveChangesAsync() == 0)
            // ! Return the error
            return Result.Failure(new FailedOperationException("Failed to save the activity to the database."));

        // * Return the success result
        return Result.Success();
    }
}