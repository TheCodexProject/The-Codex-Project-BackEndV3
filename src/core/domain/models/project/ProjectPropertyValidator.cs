using domain.exceptions;
using domain.models.resource;
using domain.models.workItem;
using domain.shared;
using OperationResult;

namespace domain.models.project;

/// <summary>
/// This class is responsible for validating the properties of a Project.
/// </summary>
public static class ProjectPropertyValidator
{
    public static Result<string> ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result<string>.Failure(
                new InvalidArgumentException("Project title cannot be empty, please provide a title."));
        }

        return title.Length switch
        {
            < 3 => Result<string>.Failure(
                new InvalidArgumentException(
                    "Project title is too short, please provide a title with at least 3 characters.")),
            > 75 => Result<string>.Failure(
                new InvalidArgumentException(
                    "Project title is too long, please provide a title with at most 100 characters.")),
            _ => Result<string>.Success(title)
        };
    }

    public static Result<string> ValidateDescription(string description)
    {
        return description.Length switch
        {
            > 500 => Result<string>.Failure(new InvalidArgumentException(
                "Project description is too long, please provide a description with at most 500 characters.")),
            _ => Result<string>.Success(description)
        };
    }

    public static Result<Status> ValidateStatus(Status status)
    {
        return status == Status.None
            ? Result<Status>.Failure(
                new InvalidArgumentException("Project status cannot be empty, please provide a status."))
            : Result<Status>.Success(status);
    }

    public static Result<Priority> ValidatePriority(Priority priority)
    {
        return priority == Priority.None
            ? Result<Priority>.Failure(
                new InvalidArgumentException("Project priority cannot be empty, please provide a priority."))
            : Result<Priority>.Success(priority);
    }

    public static Result<(DateTime start, DateTime end)> ValidateTimeRange(DateTime start, DateTime end)
    {
        // ? Is the start or end date not set?
        if (start == DateTime.MinValue || end == DateTime.MinValue)
        {
            return Result<(DateTime start, DateTime end)>.Failure(
                new InvalidArgumentException("Project time range is invalid, please provide a valid time range."));
        }

        // ? Is the start date in the past?
        if (start < DateTime.Today)
        {
            return Result<(DateTime start, DateTime end)>.Failure(
                new InvalidArgumentException("Project start date is in the past, please provide a valid start date."));
        }

        // ? Is the start date after the end date?
        if (start > end)
        {
            return Result<(DateTime start, DateTime end)>.Failure(
                new InvalidArgumentException(
                    "Project start date is after the end date, please provide a valid start date."));
        }

        // ? Is the start date before the end date?
        if (end < start)
        {
            return Result<(DateTime start, DateTime end)>.Failure(
                new InvalidArgumentException(
                    "Project end date is before the start date, please provide a valid end date."));
        }

        // ? Is the start date the same as the end date?
        if (start == end)
        {
            return Result<(DateTime start, DateTime end)>.Failure(
                new InvalidArgumentException(
                    "Project start date is the same as the end date, please provide a valid start date."));
        }

        return Result<(DateTime start, DateTime end)>.Success((start, end));
    }

    public static Result ValidateAddWorkItem(WorkItem? workItem, List<WorkItem> workItems)
    {
        // ? Is the work item null or empty?
        if (workItem == null)
        {
            return Result.Failure(new InvalidArgumentException("The provided work item is invalid. Work item cannot be null."));
        }

        // ? Does the work item already exist in the list?
        return workItems.Contains(workItem)
            ? Result.Failure(new InvalidArgumentException("The provided work item already exists in the list."))
            : Result.Success();
    }

    public static Result ValidateRemoveWorkItem(WorkItem? workItem, List<WorkItem> workItems)
    {
        // ? Is the work item null or empty?
        if (workItem == null)
        {
            return Result.Failure(new InvalidArgumentException("The provided work item is invalid. Work item cannot be null."));
        }

        // ? Does the work item exist in the list?
        return workItems.Contains(workItem)
            ? Result.Success()
            : Result.Failure(new InvalidArgumentException("The provided work item does not exist in the list."));
    }

    public static Result ValidateAddResource(Resource? resource, List<Resource> resources)
    {
        // ? Is the resource null or empty?
        if (resource == null)
        {
            return Result.Failure(new InvalidArgumentException("The provided resource is invalid. Resource cannot be null."));
        }

        // ? Does the resource already exist in the list?
        return resources.Contains(resource)
            ? Result.Failure(new InvalidArgumentException("The provided resource already exists in the list."))
            : Result.Success();
    }
    
    public static Result ValidateRemoveResource(Resource? resource, List<Resource> resources)
    {
        // ? Is the resource null or empty?
        if (resource == null)
        {
            return Result.Failure(new InvalidArgumentException("The provided resource is invalid. Resource cannot be null."));
        }

        // ? Does the resource exist in the list?
        return resources.Contains(resource)
            ? Result.Success()
            : Result.Failure(new InvalidArgumentException("The provided resource does not exist in the list."));
    }
}