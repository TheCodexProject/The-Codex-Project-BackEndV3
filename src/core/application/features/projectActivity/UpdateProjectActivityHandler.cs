using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using OperationResult;

namespace application.features.projectActivity;

public class UpdateProjectActivityHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateProjectActivityCommand>
{
    public async Task<Result> HandleAsync(UpdateProjectActivityCommand command)
    {
        // * Find the project
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

        // * Update the activity
        if (IsChanged(activity.Title, command.Title))
            activity.UpdateTitle(command.Title);

        if (IsChanged(activity.Description, command.Description))
            activity.UpdateDescription(command.Description);

        if (command.ItemsToAdd != null)
        {
            // * List of any errors that might occur while adding items to the activity
            List<Exception> errors = new();

            // Loop through the items to add and add them to the activity
            foreach (var item in command.ItemsToAdd)
            {
                // * Find the item in the database
                var dbItem = await unitOfWork.WorkItems.GetByIdAsync(item);

                // ? Was the item found?
                if (dbItem is null)
                {
                    // ! Return the error
                    errors.Add(new NotFoundException("The item could not be found."));
                }
                else
                {
                    // * Add the item to the activity
                    activity.AddWorkItem(dbItem);
                }
            }

            // ? Were there any errors?
            if (errors.Count > 0)
                // ! Return the error
                return Result.Failure(errors.ToArray());
        }

        if (command.ItemsToRemove != null)
        {
            // * List of any errors that might occur while removing items from the activity
            List<Exception> errors = new();

            // Loop through the items to remove and remove them from the activity
            foreach (var item in command.ItemsToRemove)
            {
                // * Find the item in the database
                var dbItem = await unitOfWork.WorkItems.GetByIdAsync(item);

                // ? Was the item found?
                if (dbItem is null)
                {
                    // ! Return the error
                    errors.Add(new NotFoundException("The item could not be found."));
                }
                else
                {
                    // * Remove the item from the activity
                    activity.RemoveWorkItem(dbItem);
                }
            }

            // ? Were there any errors?
            if (errors.Count > 0)
                // ! Return the error
                return Result.Failure(errors.ToArray());
        }

        // * Update the activity in the database
        unitOfWork.ProjectActivities.Update(activity);

        // ? Did the save fail?
        if (await unitOfWork.SaveChangesAsync() == 0)
            // ! Return the error
            return Result.Failure(new FailedOperationException("Failed to save the activity to the database."));

        // * Return the success result
        command.ProjectActivity = activity;
        return Result.Success();
    }

    private static bool IsChanged(string? value, string? newValue)
    {
        return value != newValue && !string.IsNullOrWhiteSpace(newValue);
    }
}