using domain.exceptions;
using domain.models.projectActivity.value;
using domain.models.workItem;
using OperationResult;

namespace domain.models.projectActivity;

/// <summary>
/// The "handler" of validation for the ProjectPhase class.
/// </summary>
public static class ProjectActivityPropertyValidator
{
    public static Result<string> ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result<string>.Failure(
                new InvalidArgumentException("Project phase title cannot be empty, please provide a title."));
        }

        return title.Length switch
        {
            < 3 => Result<string>.Failure(
                new InvalidArgumentException(
                    "Project phase title is too short, please provide a title with at least 3 characters.")),
            > 75 => Result<string>.Failure(
                new InvalidArgumentException(
                    "Project phase title is too long, please provide a title with at most 75 characters.")),
            _ => Result<string>.Success(title)
        };
    }

    public static Result<string> ValidateDescription(string description)
    {
        return description.Length switch
        {
            > 500 => Result<string>.Failure(new InvalidArgumentException(
                "Project phase description is too long, please provide a description with at most 500 characters.")),
            _ => Result<string>.Success(description)
        };
    }

    public static Result<ProjectActivityType> ValidateType(ProjectActivityType type)
    {
        return type == ProjectActivityType.None
            ? Result<ProjectActivityType>.Failure(
                new InvalidArgumentException("Project phase type cannot be empty, please provide a type."))
            : Result<ProjectActivityType>.Success(type);
    }

    public static Result ValidateAddWorkItem(WorkItem? workItem, List<WorkItem> workItems)
    {
        // ? Is the work item null or empty?
        if (workItem == null)
        {
            return Result.Failure(new InvalidArgumentException("Work item cannot be empty, please provide a work item."));
        }

        // ? Is the work item already in the list?
        return workItems.Contains(workItem)
            ? Result.Failure(new InvalidArgumentException("The provided work item already exists in the list."))
            : Result.Success();
    }

    public static Result ValidateRemoveWorkItem(WorkItem? workItem, List<WorkItem> workItems)
    {
        // ? Is the work item null or empty?
        if (workItem == null)
        {
            return Result.Failure(new InvalidArgumentException("Work item cannot be empty, please provide a work item."));
        }

        // ? Is the work item in the list?
        return !workItems.Contains(workItem)
            ? Result.Failure(new InvalidArgumentException("The provided work item does not exist in the list."))
            : Result.Success();
    }
}