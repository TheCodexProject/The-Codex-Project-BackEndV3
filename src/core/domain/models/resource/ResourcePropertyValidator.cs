using domain.exceptions;
using domain.models.resource.values;
using OperationResult;

namespace domain.models.resource;

public static class ResourcePropertyValidator
{
    public static Result<string> ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result<string>.Failure(
                new InvalidArgumentException("Resource title cannot be empty, please provide a title."));
        }

        return title.Length switch
        {
            < 3 => Result<string>.Failure(
                new InvalidArgumentException(
                    "Resource title is too short, please provide a title with at least 3 characters.")),
            > 75 => Result<string>.Failure(
                new InvalidArgumentException(
                    "Resource title is too long, please provide a title with at most 75 characters.")),
            _ => Result<string>.Success(title)
        };
    }

    public static Result<string> ValidateDescription(string description)
    {
        return description.Length switch
        {
            > 500 => Result<string>.Failure(new InvalidArgumentException(
                "Resource description is too long, please provide a description with at most 500 characters.")),
            _ => Result<string>.Success(description)
        };
    }

    public static Result<string> ValidateUrl(string url)
    {
        return string.IsNullOrWhiteSpace(url)
            ? Result<string>.Failure(
                new InvalidArgumentException("Resource URL cannot be empty, please provide a URL."))
            : Result<string>.Success(url);
    }

    public static Result<ResourceType> ValidateType(ResourceType type)
    {
        return type == ResourceType.None
            ? Result<ResourceType>.Failure(
                new InvalidArgumentException("Resource type cannot be empty, please provide a type."))
            : Result<ResourceType>.Success(type);
    }

    public static Result<ResourceLevel> ValidateLevel(ResourceLevel level)
    {
        return level == ResourceLevel.None
            ? Result<ResourceLevel>.Failure(
                new InvalidArgumentException("Resource level cannot be empty, please provide a level."))
            : Result<ResourceLevel>.Success(level);
    }
}