using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.workItem;

public class GetAllWorkItemsHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetAllWorkItemsCommand>
{
    public async Task<Result> HandleAsync(GetAllWorkItemsCommand command)
    {
        // * Get all the work items
        var workItems = await unitOfWork.WorkItems.GetAllAsync();

        // ? Were there any work items?
        var enumerable = workItems.ToList();

        if (enumerable.Count == 0)
            // ! Return the error
            return Result.Failure(new NotFoundException("No work items were found in the database."));

        // * Return the work items
        command.WorkItems = enumerable;
        return Result.Success();
    }
}