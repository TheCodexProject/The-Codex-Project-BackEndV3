using domain.exceptions;
using domain.models.resource;
using OperationResult;

namespace application.appEntry.commands.resource;

public class GetResourceCommand
{
    public Guid Id { get; set; }

    public Resource? Resource { get; set; } = null;

    private GetResourceCommand(Guid id)
    {
        Id = id;
    }

    public static Result<GetResourceCommand> Create(string id)
    {
        var validationResult = Validate(id);

        if (validationResult.IsFailure)
            return Result<GetResourceCommand>.Failure(validationResult.Errors.ToArray());

        return new GetResourceCommand(validationResult);
    }

    private static Result<Guid> Validate(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return Result<Guid>.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        return Result<Guid>.Success(parsedId);
    }
}