using domain.exceptions;
using domain.models.project;
using OperationResult;

namespace application.appEntry.commands.project;

public class GetProjectCommand
{
    public Guid Id { get; set; }

    public Project? Project { get; set; } = null;

    private GetProjectCommand(Guid id)
    {
        Id = id;
    }

    public static Result<GetProjectCommand> Create(string id)
    {
        var validationResult = Validate(id);

        if (validationResult.IsFailure)
            return Result<GetProjectCommand>.Failure(validationResult.Errors.ToArray());

        return new GetProjectCommand(new Guid(id));
    }

    private static Result Validate(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        return Result.Success();
    }
}