using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.workItem.values;
using domain.shared;
using OperationResult;

namespace application.features.workItem;

public class UpdateWorkItemHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateWorkItemCommand>
{
    public async Task<Result> HandleAsync(UpdateWorkItemCommand command)
    {
        // * Get the work item from the database
        var workItem = await unitOfWork.WorkItems.GetByIdAsync(command.Id);

        // ? Was the work item found?
        if (workItem is null)
            // ! Return the errors
            return Result.Failure(new NotFoundException("The work item was not found."));

        // * Update the work item
        if (IsChanged(workItem.Title, command.Title))
            workItem.UpdateTitle(command.Title);

        if (IsChanged(workItem.Description, command.Description))
            workItem.UpdateDescription(command.Description);

        if(command.WorkItemStatus.HasValue && (command.WorkItemStatus != workItem.Status || command.WorkItemStatus != Status.None))
            workItem.UpdateStatus(command.WorkItemStatus.Value);

        if(command.WorkItemPriority.HasValue && (workItem.Priority != command.WorkItemPriority || command.WorkItemPriority != Priority.None))
            workItem.UpdatePriority(command.WorkItemPriority.Value);

        if(command.Type.HasValue && (workItem.Type != command.Type || command.Type != ItemType.None))
            workItem.UpdateType(command.Type.Value);

        if (command.AssigneeId.HasValue && command.AssigneeId != Guid.Empty)
        {
            // Find the assignee in the database
            var assignee = await unitOfWork.Users.GetByIdAsync(command.AssigneeId.Value);

            // ? If the assignee does not exist
            if (assignee == null)
                return Result.Failure(new NotFoundException("The given assignee could not be found"));

            // Update the assignee
            workItem.UpdateAssignedTo(assignee);
        }

        if (command.SubItemsToAdd != null)
        {
            // Loop through the subitems to add
            foreach (var subItem in command.SubItemsToAdd)
            {
                // Find the subitem in the database
                var toAdd = await unitOfWork.WorkItems.GetByIdAsync(subItem);

                // ? If the subitem does not exist
                if (toAdd == null)
                    return Result.Failure(new NotFoundException("The given sub item could not be found"));

                // Add the subitem to the work item
                workItem.AddSubitem(toAdd);
            }
        }

        if (command.SubItemsToRemove != null)
        {
            // Loop through the subitems to remove
            foreach (var subItem in command.SubItemsToRemove)
            {
                // Find the subitem in the database
                var toRemove = await unitOfWork.WorkItems.GetByIdAsync(subItem);

                // ? If the subitem does not exist
                if (toRemove == null)
                    return Result.Failure(new NotFoundException("The given sub item could not be found"));

                // Remove the subitem from the work item
                workItem.RemoveSubitem(toRemove);
            }
        }


        // * Save the changes
        await unitOfWork.SaveChangesAsync();

        // * Return the work item
        command.WorkItem = workItem;
        return Result.Success();
    }

    private static bool IsChanged(string? value, string? newValue)
    {
        return value != newValue && !string.IsNullOrWhiteSpace(newValue);
    }
}