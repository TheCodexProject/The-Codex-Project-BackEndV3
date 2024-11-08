﻿using System.ComponentModel.DataAnnotations;
using domain.models.resource.values;
using OperationResult;

namespace domain.models.resource;

public class Resource
{
    // # METADATA #
    [Key]
    public Guid Id { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; }

    public DateTime UpdatedAt { get; private set; } = DateTime.MinValue;
    public string UpdatedBy { get; private set; } = string.Empty;

    // # PROPERTIES #
    [Required]
    [MaxLength(75)]
    [MinLength(3)]
    public string Title { get; private set; }

    [MaxLength(500)]
    public string Description { get; private set; } = string.Empty;

    [Required]
    public string Url { get; private set; }

    public ResourceType Type { get; private set; } = ResourceType.None;

    public ResourceLevel Level { get; private set; } = ResourceLevel.None;

    /// <summary>
    /// The ID of the "Owner" of this resource. This is based on the Resource's Level.
    /// It can either be a Organization ID, Workspace ID or Project ID.
    /// </summary>
    public Guid OwnerId { get; private set; }
    
    // # CONSTRUCTORS #

    // NOTE: EF Core constructor
    private Resource() { }

    private Resource(string title, string url, Guid ownerId, ResourceLevel level)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        CreatedBy = "Todo: Implement";

        Title = title;
        Url = url;
        OwnerId = ownerId;
        Level = level;
    }

    public static Result<Resource> Create(string title, string url, Guid ownerId, ResourceLevel level)
    {
        // ! Validate the resource's input here.
        var validationResult = Validate(title, url, level);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
        {
            return Result<Resource>.Failure(validationResult.Errors.ToArray());
        }

        return Result<Resource>.Success(new Resource(title, url, ownerId, level));
    }

    private static Result Validate(string title, string url, ResourceLevel level)
    {
        // * List for errors encountered during validation.
        List<Exception> errors = [];

        // ! Validate the title
        var titleValidation = ResourcePropertyValidator.ValidateTitle(title);

        // ? Is the title validation a failure?
        if (titleValidation.IsFailure)
        {
            errors.AddRange(titleValidation.Errors);
        }

        // ! Validate the url
        var urlValidation = ResourcePropertyValidator.ValidateUrl(url);

        // ? Is the url validation a failure?
        if (urlValidation.IsFailure)
        {
            errors.AddRange(urlValidation.Errors);
        }
        
        // ! Validate the level
        var levelValidation = ResourcePropertyValidator.ValidateLevel(level);
        
        // ? Is the level validation a failure?
        if (levelValidation.IsFailure)
        {
            errors.AddRange(levelValidation.Errors);
        }

        return errors.Count != 0
            ? Result.Failure(errors.ToArray())
            : Result.Success();
    }

    // # METHODS #
    public Result UpdateTitle(string title)
    {
        // ? Validate the input.
        var result = ResourcePropertyValidator.ValidateTitle(title);

        // ? Is the validation a failure?
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors.ToArray());
        }

        Title = title;
        return Result.Success();
    }

    public Result UpdateDescription(string description)
    {
        // ? Validate the input.
        var result = ResourcePropertyValidator.ValidateDescription(description);

        // ? Is the validation a failure?
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors.ToArray());
        }

        Description = description;
        return Result.Success();
    }

    public Result UpdateUrl(string url)
    {
        // ? Validate the input.
        var result = ResourcePropertyValidator.ValidateUrl(url);

        // ? Is the validation a failure?
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors.ToArray());
        }

        Url = url;
        return Result.Success();
    }

    public Result UpdateType(ResourceType type)
    {
        // ? Validate the input.
        var result = ResourcePropertyValidator.ValidateType(type);

        // ? Is the validation a failure?
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors.ToArray());
        }

        Type = type;
        return Result.Success();
    }

    public Result UpdateLevel(ResourceLevel level)
    {
        // ? Validate the input.
        var result = ResourcePropertyValidator.ValidateLevel(level);

        // ? Is the validation a failure?
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors.ToArray());
        }

        Level = level;
        return Result.Success();
    }
}