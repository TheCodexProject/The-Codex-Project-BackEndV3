using domain.exceptions;
using domain.models.organization;
using OperationResult;

namespace application.appEntry.commands.organization;

public class GetOrganizationCommand
{
    public Guid Id { get; set; }
    public Organization? Organization { get; set; } = null;

    private GetOrganizationCommand(Guid id)
    {
        Id = id;
    }

    public static Result<GetOrganizationCommand> Create(string id)
    {
        var validationResult = Validate(id);

        if (validationResult.IsFailure)
            return Result<GetOrganizationCommand>.Failure(validationResult.Errors.ToArray());

        return new GetOrganizationCommand(new Guid(id));
    }

    private static Result Validate(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        return Result.Success();
    }


}