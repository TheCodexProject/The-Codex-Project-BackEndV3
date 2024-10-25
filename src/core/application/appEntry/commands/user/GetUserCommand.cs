using domain.exceptions;
using domain.models.user;
using OperationResult;

namespace application.appEntry.commands.user;

public class GetUserCommand
{
    public Guid Id { get; set; }
    public User? User { get; set; } = null;

    private GetUserCommand(Guid id)
    {
        Id = id;
    }

    public static Result<GetUserCommand> Create(string id)
    {
        var validationResult = Validate(id);

        if (validationResult.IsFailure)
            return Result<GetUserCommand>.Failure(validationResult.Errors.ToArray());

        return new GetUserCommand(validationResult);
    }

    private static Result<Guid> Validate(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return Result<Guid>.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        return Result<Guid>.Success(parsedId);
    }
}