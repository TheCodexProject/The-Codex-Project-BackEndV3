using domain.exceptions;
using OperationResult;

namespace application.appEntry.commands.user;

public class DeleteUserCommand
{
    public Guid Id { get; set; }

    private DeleteUserCommand(Guid id)
    {
        Id = id;
    }

    public static Result<DeleteUserCommand> Create(string id)
    {
        var validationResult = Validate(id);

        if (validationResult.IsFailure)
            return Result<DeleteUserCommand>.Failure(validationResult.Errors.ToArray());

        return new DeleteUserCommand(validationResult.Value);
    }

    private static Result<Guid> Validate(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return Result<Guid>.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        return Result<Guid>.Success(parsedId);
    }
}