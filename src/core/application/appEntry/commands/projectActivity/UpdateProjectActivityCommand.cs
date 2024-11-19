using domain.exceptions;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using OperationResult;

namespace application.appEntry.commands.projectActivity;

public class UpdateProjectActivityCommand
{
    // NOTE: Given information
    public Guid ActivityId { get; }
    public Guid ProjectId { get; }
    public ProjectActivityType Type { get; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Guid>? ItemsToAdd { get; set; }
    public List<Guid>? ItemsToRemove { get; set; }

    // NOTE: Result information
    public ProjectActivity? ProjectActivity { get; set; } = null;

    private UpdateProjectActivityCommand(Guid activityId, Guid projectId, ProjectActivityType type,string? title, string? description, List<Guid>? itemsToAdd, List<Guid>? itemsToRemove)
    {
        ActivityId = activityId;
        ProjectId = projectId;
        Type = type;
        Title = title?? string.Empty;
        Description = description?? string.Empty;
        ItemsToAdd = itemsToAdd?? [];
        ItemsToRemove = itemsToRemove?? [];
    }

    public static Result<UpdateProjectActivityCommand> Create(string activityId, string projectId, ProjectActivityType type, string? title, string? description, List<string>? itemsToAdd, List<string>? itemsToRemove)
    {
        // ! Validate the input
        var validationResult = Validate(activityId, projectId, title, description, itemsToAdd, itemsToRemove);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<UpdateProjectActivityCommand>.Failure(validationResult.Errors.ToArray());

        // * Convert the string list to a Guid list
        List<Guid>? itemsToAddGuid = itemsToAdd?.ConvertAll(Guid.Parse);
        List<Guid>? itemsToRemoveGuid = itemsToRemove?.ConvertAll(Guid.Parse);

        // * Return the newly created command
        return new UpdateProjectActivityCommand(new Guid(activityId), new Guid(projectId), type, title, description, itemsToAddGuid, itemsToRemoveGuid);
    }

    private static Result Validate(string activityId, string projectId, string? title, string? description, List<string>? itemsToAdd, List<string>? itemsToRemove)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = [];

        // ! Validate the ID
        if (!Guid.TryParse(activityId, out var _))
            exceptions.Add(new FailedOperationException("The given Activity ID could not be parsed into a GUID"));

        // ! Validate the owner ID (Guid)
        if (!Guid.TryParse(projectId, out var _))
            exceptions.Add(new FailedOperationException("The given Project ID could not be parsed into a GUID"));

        // ! Validate the title
        if (title is not null)
        {
            // * Validate the title
            var titleValidation = ProjectActivityPropertyValidator.ValidateTitle(title);

            // ? Were there any errors during the title validation?
            if (titleValidation.IsFailure)
                exceptions.AddRange(titleValidation.Errors);
        }

        // ! Validate the description
        if (description is not null)
        {
            // * Validate the description
            var descriptionValidation = ProjectActivityPropertyValidator.ValidateDescription(description);

            // ? Were there any errors during the description validation?
            if (descriptionValidation.IsFailure)
                exceptions.AddRange(descriptionValidation.Errors);
        }

        if (itemsToAdd != null)
        {
            foreach (var item in itemsToAdd)
            {
                if (!Guid.TryParse(item, out var _))
                    exceptions.Add(new FailedOperationException($"The item ID '{item}' could not be parsed into a GUID"));
            }
        }

        if (itemsToRemove != null)
        {
            foreach (var item in itemsToRemove)
            {
                if (!Guid.TryParse(item, out var _))
                    exceptions.Add(new FailedOperationException($"The item ID '{item}' could not be parsed into a GUID"));
            }
        }

        // ? Were there any exceptions?
        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray()) // ! Return errors.
            : Result.Success(); // * Return success.
    }



}