using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.workItem;

public class GetWorkItemHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetWorkItemCommand>
{
    public async Task<Result> HandleAsync(GetWorkItemCommand command)
    {
        // * Get the work item from the database
        var workItem = await unitOfWork.WorkItems.GetByIdAsync(command.Id);

        // ? Was the work item found?
        if (workItem is null)
            // ! Return the errors
            return Result.Failure(new NotFoundException("The work item was not found."));

        // * Return the work item
        command.WorkItem = workItem;
        return Result.Success();
    }
}
