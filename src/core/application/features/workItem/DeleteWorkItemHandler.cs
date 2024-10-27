using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.workItem;

public class DeleteWorkItemHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteWorkItemCommand>
{
    public async Task<Result> HandleAsync(DeleteWorkItemCommand command)
    {
        // ? Does the work item exist?
        var workItem = await unitOfWork.WorkItems.GetByIdAsync(command.Id);

        // ? If the work item does not exist
        if (workItem == null)
            return Result.Failure(new NotFoundException("The given workItem could not be found"));

        // * Delete the workItem
        unitOfWork.WorkItems.Remove(workItem);

        // ? Was the work item deleted?
        if (await unitOfWork.SaveChangesAsync() == 0)
            return Result.Failure(new FailedOperationException("WorkItem could not be deleted"));

        // * Return success
        return Result.Success();
    }
}