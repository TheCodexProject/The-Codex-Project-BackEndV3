using domain.exceptions;
using domain.models.resource.values;
using OperationResult;

namespace application.appEntry.commands.resource;

public class DeleteResourceCommand
{
    public Guid Id { get; set; }
    public ResourceLevel Level { get; set; }
    public Guid OwnerId { get; set; }

    private DeleteResourceCommand(Guid id)
    {
        Id = id;
    }

    public static Result<DeleteResourceCommand> Create(string id)
    {
        // ! Validate the input
        var validationResult = Validate(id);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<DeleteResourceCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command
        return new DeleteResourceCommand(validationResult.Value);
    }

    private static Result<Guid> Validate(string id)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = [];

        // ! Validate the ID
        if (!Guid.TryParse(id, out var parsedId))
            exceptions.Add(new FailedOperationException("The given ID could not be parsed into a GUID"));

        // ? Were there any exceptions?
        return exceptions.Count != 0
            ? Result<Guid>.Failure(exceptions.ToArray()) // * Yes: Return the exceptions
            : Result<Guid>.Success(parsedId); // * No: Return the parsed ID
    }
}