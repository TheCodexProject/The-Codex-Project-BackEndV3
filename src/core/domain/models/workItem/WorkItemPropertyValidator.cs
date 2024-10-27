using domain.exceptions;
using domain.models.user;
using domain.models.workItem.values;
using domain.shared;
using OperationResult;

namespace domain.models.workItem;

/// <summary>
/// This class is responsible for validating the properties of a work item.
/// </summary>
public static class WorkItemPropertyValidator
{
    public static Result<string> ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result<string>.Failure(new InvalidArgumentException("Work item title cannot be empty, please provide a title."));
        }

        return title.Length switch
        {
            < 3 => Result<string>.Failure(new InvalidArgumentException("Work item title is too short, please provide a title with at least 3 characters.")),
            > 75 => Result<string>.Failure(new InvalidArgumentException("Work item title is too long, please provide a title with at most 75 characters.")),
            _ => Result<string>.Success(title)
        };
    }

    public static Result<string> ValidateDescription(string description)
    {
        return description.Length switch
        {
            > 500 => Result<string>.Failure(new InvalidArgumentException("Work item description is too long, please provide a description with at most 500 characters.")),
            _ => Result<string>.Success(description)
        };
    }

    public static Result<Status> ValidateStatus(Status status)
    {
        return status == Status.None ? Result<Status>.Failure(new InvalidArgumentException("Work item status cannot be empty, please provide a status.")) : Result<Status>.Success(status);
    }

    public static Result<Priority> ValidatePriority(Priority priority)
    {
        return priority == Priority.None ? Result<Priority>.Failure(new InvalidArgumentException("Work item priority cannot be empty, please provide a priority.")) : Result<Priority>.Success(priority);
    }

    public static Result<ItemType> ValidateType(ItemType type)
    {
        return type == ItemType.None ? Result<ItemType>.Failure(new InvalidArgumentException("Work item type cannot be empty, please provide a type.")) : Result<ItemType>.Success(type);
    }

    public static Result<User> ValidateAssignedTo(User? assignedTo)
    {
        return assignedTo == null ? Result<User>.Failure(new InvalidArgumentException("Work item assigned to cannot be empty, please provide a user.")) : Result<User>.Success(assignedTo);
    }

    public static Result ValidateAddSubitem(WorkItem? subitem, List<WorkItem> subitems)
    {
        // ? Is the subitem null or empty?
        if (subitem == null)
            return Result.Failure(new InvalidArgumentException("The provided subitem is invalid. Subitem cannot be null."));

        // ? Does the subitem already exist in the list?
        return subitems.Contains(subitem) ?
            Result.Failure(new InvalidArgumentException("The provided subitem already exists in the list."))
            : Result.Success();
    }

    public static Result ValidateRemoveSubitem(WorkItem? subitem, List<WorkItem> subitems)
    {
        // ? Is the subitem null or empty?
        if (subitem == null)
            return Result.Failure(new InvalidArgumentException("The provided subitem is invalid. Subitem cannot be null."));

        // ? Does the subitem exist in the list?
        return subitems.Contains(subitem) ?
            Result.Success()
            : Result.Failure(new InvalidArgumentException("The provided subitem does not exist in the list."));
    }

    // TODO: TO BE EXTENDED
}