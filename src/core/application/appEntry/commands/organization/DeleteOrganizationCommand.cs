using domain.exceptions;
using OperationResult;

namespace application.appEntry.commands.organization;

public class DeleteOrganizationCommand
{
    public Guid Id { get; set; }

    private DeleteOrganizationCommand(Guid id)
    {
        Id = id;
    }

    public static Result<DeleteOrganizationCommand> Create(string id)
    {
        var validationResult = Validate(id);

        if (validationResult.IsFailure)
            return Result<DeleteOrganizationCommand>.Failure(validationResult.Errors.ToArray());

        return new DeleteOrganizationCommand(validationResult);
    }

    private static Result<Guid> Validate(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return Result<Guid>.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        return Result<Guid>.Success(parsedId);
    }
}