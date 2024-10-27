using domain.exceptions;
using domain.models.workItem;
using domain.models.workspace;
using OperationResult;

namespace application.appEntry.commands.workItem;

public class GetWorkItemCommand
{
    public Guid Id { get; set; }

    public WorkItem? WorkItem { get; set; } = null;

    private GetWorkItemCommand(Guid id)
    {
        Id = id;
    }

    public static Result<GetWorkItemCommand> Create(string id)
    {
        var validationResult = Validate(id);

        if (validationResult.IsFailure)
            return Result<GetWorkItemCommand>.Failure(validationResult.Errors.ToArray());

        return new GetWorkItemCommand(new Guid(id));
    }

    private static Result Validate(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        return Result.Success();
    }
    
}