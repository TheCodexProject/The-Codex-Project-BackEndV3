using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using OperationResult;

namespace application.features.projectActivity;

public class GetAllProjectActivitiesHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetAllProjectActivitiesCommand>
{
    public async Task<Result> HandleAsync(GetAllProjectActivitiesCommand command)
    {
        // * Find the project
        var project = await unitOfWork.Projects.GetByIdAsync(command.ProjectId);

        // ? Was the project found?
        if (project is null)
            // ! Return the error
            return Result.Failure(new NotFoundException("The project could not be found."));

        // * Initialize a list for the activities
        List<ProjectActivity> activities = new();
        switch (command.Type)
        {
            case ProjectActivityType.Milestone:
                activities = project.Milestones.ToList();
                break;
            case ProjectActivityType.Iteration:
                activities = project.Iterations.ToList();
                break;
        }

        // ? Were there any activities?
        if (activities.Count == 0)
            // ! Return the error
            return Result.Failure(new NotFoundException("No activities were found in the database."));

        // * Return the activities
        command.ProjectActivities = activities;
        return Result.Success();
    }
}