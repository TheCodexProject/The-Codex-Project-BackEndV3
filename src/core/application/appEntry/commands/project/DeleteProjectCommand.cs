using domain.exceptions;
using OperationResult;

namespace application.appEntry.commands.project;

public class DeleteProjectCommand
{
    public Guid Id { get; set; }

    private DeleteProjectCommand(Guid id)
    {
        Id = id;
    }

    public static Result<DeleteProjectCommand> Create(string id)
    {
        var validationResult = Validate(id);

        if (validationResult.IsFailure)
            return Result<DeleteProjectCommand>.Failure(validationResult.Errors.ToArray());

        return new DeleteProjectCommand(new Guid(id));
    }

    private static Result Validate(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        return Result.Success();
    }
}