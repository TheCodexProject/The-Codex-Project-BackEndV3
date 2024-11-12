using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.workItem;

public class GetProjectWorkItemsHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetProjectWorkItemsCommand>
{
    public async Task<Result> HandleAsync(GetProjectWorkItemsCommand command)
    {
        // * Get all the work items
        var workItems = await unitOfWork.WorkItems.GetAllAsync();

        // ? Were there any work items?
        var enumerable = workItems.ToList();

        // * Filter the work items by project ID
        var projectWorkItems = enumerable.Where(wi => wi.Project.Id == command.ProjectId).ToList();

        if (projectWorkItems.Count == 0)
            // ! Return the error
            return Result.Failure(new NotFoundException("No work items were found in the database for the specified project."));

        // * Return the work items
        command.WorkItems = projectWorkItems;
        return Result.Success();
    }
}