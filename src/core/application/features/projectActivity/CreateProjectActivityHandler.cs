using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using OperationResult;

namespace application.features.projectActivity;

public class CreateProjectActivityHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateProjectActivityCommand>
{
    public async Task<Result> HandleAsync(CreateProjectActivityCommand command)
    {
        // * First find the project
        var project = await unitOfWork.Projects.GetByIdAsync(command.ProjectId);

        // ? Was the project found?
        if (project is null)
            // ! Return the error
            return Result.Failure(new NotFoundException("The project could not be found."));

        // * Determine the project activity type (It is going to be either a milestone or an iteration)
        // That is why we can ignore the warning about the activity being null
        ProjectActivity activity = null!;

        switch (command.Type)
        {
            case ProjectActivityType.Milestone:
                var result = ProjectActivityBuilder.BuildMilestone(project, command.Title);

                // ? Was the milestone created successfully?
                if (result.IsFailure)
                    // ! Return the error
                    return Result.Failure(result.Errors.ToArray());

                activity = result.Value;
                break;
            case ProjectActivityType.Iteration:
                result = ProjectActivityBuilder.BuildIteration(project, command.Title);

                // ? Was the iteration created successfully?
                if (result.IsFailure)
                    // ! Return the error
                    return Result.Failure(result.Errors.ToArray());

                activity = result.Value;
                break;
        }

        // * Add the activity to the project
        var addActivityResult = project.AddActivity(activity);

        // ? Were there any validation errors?
        if (addActivityResult.IsFailure)
            // ! Return the error
            return Result.Failure(addActivityResult.Errors.ToArray());

        // * Save the activity to the database
        await unitOfWork.ProjectActivities.AddAsync(activity);

        // ? Did the save fail?
        if (await unitOfWork.SaveChangesAsync() == 0)
            // ! Return the error
            return Result.Failure(new FailedOperationException("Failed to save the activity to the database."));

        // * Return the success result
        command.Id = activity.Id;
        return Result.Success();
    }
}