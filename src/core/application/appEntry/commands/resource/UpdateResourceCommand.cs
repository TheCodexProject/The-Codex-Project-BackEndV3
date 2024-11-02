using domain.exceptions;
using domain.models.resource;
using domain.models.resource.values;
using OperationResult;

namespace application.appEntry.commands.resource;

public class UpdateResourceCommand
{
    // NOTE: Given information
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public ResourceLevel Level { get; set; }

    // NOTE: Result information
    public Resource? Resource { get; set; } = null;
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public ResourceType? Type { get; set; }

    private UpdateResourceCommand(Guid id, Guid ownerId, ResourceLevel level,string? title, string? url, string? description, ResourceType? type)
    {
        Id = id;
        OwnerId = ownerId;
        Level = level;
        Title = title;
        Url = url;
        Description = description;
        Type = type;
    }

    public static Result<UpdateResourceCommand> Create(string id, string ownerId, ResourceLevel level, string? title, string? url, string? description, string? type)
    {
        // ! Validate the input
        var validationResult = Validate(id, ownerId, title, url, description, type);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<UpdateResourceCommand>.Failure(validationResult.Errors.ToArray());

        // * Convert the type to a ResourceType
        if (!Enum.TryParse(type, true, out ResourceType typeEnum))
        {
            typeEnum = ResourceType.None;
        }

        // * Return the newly created command
        return new UpdateResourceCommand(new Guid(id),new Guid(ownerId),level, title, url, description, typeEnum);
    }

    private static Result Validate(string id, string ownerId, string? title, string? url, string? description, string? type)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = [];

        // ! Validate the ID
        if (!Guid.TryParse(id, out var _))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        // ! Validate the ID
        if (!Guid.TryParse(ownerId, out var _))
            return Result.Failure(new FailedOperationException("The given Owner ID could not be parsed into a GUID"));

        // ! Validate the Title
        if (title is not null)
        {
            var titleValidation = ResourcePropertyValidator.ValidateTitle(title);

            if (titleValidation.IsFailure)
                exceptions.AddRange(titleValidation.Errors);
        }

        // ! Validate the URL
        if (url is not null)
        {
            var urlValidation = ResourcePropertyValidator.ValidateUrl(url);

            if (urlValidation.IsFailure)
                exceptions.AddRange(urlValidation.Errors);
        }

        // ! Validate the Description
        if (description is not null)
        {
            var descriptionValidation = ResourcePropertyValidator.ValidateDescription(description);

            if (descriptionValidation.IsFailure)
                exceptions.AddRange(descriptionValidation.Errors);
        }

        // ! Validate the Type
        if (type is not null)
        {
            // ? Is the type a valid enum value?
            if (!Enum.TryParse(typeof(ResourceType), type, out _))
                // ! If not, return an error
                exceptions.Add(new FailedOperationException("The given status is not a valid status"));

            // * Convert the type to a ResourceType
            var typeEnum = (ResourceType)Enum.Parse(typeof(ResourceType), type);

            var typeValidation = ResourcePropertyValidator.ValidateType(typeEnum);

            if (typeValidation.IsFailure)
                exceptions.AddRange(typeValidation.Errors);
        }

        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray())
            : Result.Success();
    }





}