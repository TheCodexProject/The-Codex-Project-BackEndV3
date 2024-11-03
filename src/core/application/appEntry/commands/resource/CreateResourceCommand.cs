using domain.exceptions;
using domain.models.resource;
using domain.models.resource.values;
using OperationResult;

namespace application.appEntry.commands.resource;

public class CreateResourceCommand
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string Url { get; set; }

    public Guid OwnerId { get; set; }

    public ResourceLevel Level { get; set; }

    private CreateResourceCommand(string title, string url, Guid ownerId, ResourceLevel level)
    {
        Title = title;
        Url = url;
        OwnerId = ownerId;
        Level = level;
    }

    public static Result<CreateResourceCommand> Create(string title, string url, string ownerId, ResourceLevel level)
    {
        // ! Validate the input
        var validationResult = Validate(title, url, ownerId);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<CreateResourceCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command
        return new CreateResourceCommand(title, url, new Guid(ownerId), level);
    }

    private static Result Validate(string title, string url, string ownerId)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = [];

        // ! Validate the owner ID (Guid)
        if (!Guid.TryParse(ownerId, out var parsedWorkspaceId))
            return Result.Failure(new FailedOperationException("The given Owner ID could not be parsed into a GUID"));

        // ! Validate the title
        var titleValidation = ResourcePropertyValidator.ValidateTitle(title);

        // ? Did the title validation fail?
        if (titleValidation.IsFailure)
            exceptions.AddRange(titleValidation.Errors);

        // ! Validate the URL
        var urlValidation = ResourcePropertyValidator.ValidateUrl(url);

        // ? Did the URL validation fail?
        if (urlValidation.IsFailure)
            exceptions.AddRange(urlValidation.Errors);

        ;

        // ? Were there any exceptions?
        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray()) // * Yes: Return the exceptions
            : Result.Success(); // * No: Return success
    }
}