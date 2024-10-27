using domain.exceptions;
using domain.models.workItem;
using domain.models.workItem.values;
using domain.shared;
using OperationResult;

namespace application.appEntry.commands.workItem;

public class UpdateWorkItemCommand
{
    public Guid Id { get; }

    public WorkItem? WorkItem { get; set; } = null;
    public string? Title { get; }
    public string? Description { get; }
    public Status? WorkItemStatus { get; }
    public Priority? WorkItemPriority { get; }
    public ItemType? Type { get; }

    public Guid? AssigneeId { get; }
    public List<Guid>? SubItemsToAdd { get; }
    public List<Guid>? SubItemsToRemove { get; }

    private UpdateWorkItemCommand(Guid id, string title, string description, Status workItemStatus, Priority workItemPriority, ItemType type, Guid? assigneeId, List<Guid> subItemsToAdd, List<Guid> subItemsToRemove)
    {
        Id = id;
        Title = title;
        Description = description;
        WorkItemStatus = workItemStatus;
        WorkItemPriority = workItemPriority;
        Type = type;
        AssigneeId = assigneeId;
        SubItemsToAdd = subItemsToAdd ?? new List<Guid>();
        SubItemsToRemove = subItemsToRemove ?? new List<Guid>();
    }

    public static Result<UpdateWorkItemCommand> Create(string id, string? title, string? description, string? status, string? priority, string? type, string? assignee, List<string>? subItemsToAdd, List<string>? subItemsToRemove)
    {
        // ! Validate the user's input
        var validationResult = Validate(id, title, description, status, priority, type, assignee, subItemsToAdd, subItemsToRemove);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<UpdateWorkItemCommand>.Failure(validationResult.Errors.ToArray());

        if (!Enum.TryParse(status, true, out Status statusEnum))
        {
            statusEnum = Status.None;
        }

        if (!Enum.TryParse(priority, true, out Priority priorityEnum))
        {
            priorityEnum = Priority.None;
        }

        if (!Enum.TryParse(type, true, out ItemType typeEnum))
        {
            typeEnum = ItemType.Task;
        }

        var assigneeId = assignee != null
            ? Guid.Parse(assignee)
            : Guid.Empty;

        var subItemsToAddGuids = subItemsToAdd != null
            ? subItemsToAdd.ConvertAll(Guid.Parse)
            : new List<Guid>();

        var subItemsToRemoveGuids = subItemsToRemove != null
            ? subItemsToRemove.ConvertAll(Guid.Parse)
            : new List<Guid>();

        // * Return the newly created command.
        return new UpdateWorkItemCommand(new Guid(id), title, description, statusEnum, priorityEnum, typeEnum, assigneeId, subItemsToAddGuids, subItemsToRemoveGuids);
    }

    private static Result Validate(string id, string? title, string? description, string? status, string? priority, string? type, string? assignee, List<string>? subItemsToAdd, List<string>? subItemsToRemove)
    {
        List<Exception> exceptions = [];

        // ! Validate the ID
        if (!Guid.TryParse(id, out _))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        // ! Validate the title
        if (!string.IsNullOrWhiteSpace(title))
        {
            var titleValidation = WorkItemPropertyValidator.ValidateTitle(title);

            if (titleValidation.IsFailure)
                exceptions.AddRange(titleValidation.Errors);
        }

        // ! Validate the description
        if (!string.IsNullOrWhiteSpace(description))
        {
            var descriptionValidation = WorkItemPropertyValidator.ValidateDescription(description);

            if (descriptionValidation.IsFailure)
                exceptions.AddRange(descriptionValidation.Errors);
        }

        // ! Validate the status
        if (!string.IsNullOrWhiteSpace(status))
        {
            // ? Is the status a valid enum value?
            if (!Enum.TryParse(typeof(Status), status, out _))
                // ! If not, return an error
                exceptions.Add(new FailedOperationException("The given status is not a valid status"));

            // * Convert the status to an enum
            var statusEnum = (Status)Enum.Parse(typeof(Status), status);

            var statusValidation = WorkItemPropertyValidator.ValidateStatus(statusEnum);

            if (statusValidation.IsFailure)
                exceptions.AddRange(statusValidation.Errors);
        }

        // ! Validate the priority
        if (!string.IsNullOrWhiteSpace(priority))
        {
            // ? Is the priority a valid enum value?
            if (!Enum.TryParse(typeof(Priority), priority, out _))
                // ! If not, return an error
                exceptions.Add(new FailedOperationException("The given priority is not a valid priority"));

            // * Convert the priority to an enum
            var priorityEnum = (Priority)Enum.Parse(typeof(Priority), priority);

            var priorityValidation = WorkItemPropertyValidator.ValidatePriority(priorityEnum);

            if (priorityValidation.IsFailure)
                exceptions.AddRange(priorityValidation.Errors);
        }

        // ! Validate the type
        if (!string.IsNullOrWhiteSpace(type))
        {
            // ? Is the type a valid enum value?
            if (!Enum.TryParse(typeof(ItemType), type, out _))
                // ! If not, return an error
                exceptions.Add(new FailedOperationException("The given type is not a valid type"));

            // * Convert the type to an enum
            var typeEnum = (ItemType)Enum.Parse(typeof(ItemType), type);

            var typeValidation = WorkItemPropertyValidator.ValidateType(typeEnum);

            if (typeValidation.IsFailure)
                exceptions.AddRange(typeValidation.Errors);
        }

        // ! Validate the assignee
        if (!string.IsNullOrWhiteSpace(assignee))
        {
            if (!Guid.TryParse(assignee, out _))
            {
                exceptions.Add(new FailedOperationException("The given assignee ID could not be parsed into a GUID"));
            }
        }

        // ! Validate the GUIDs to add
        if (subItemsToAdd != null)
        {
            foreach (var subItem in subItemsToAdd)
            {
                if (!Guid.TryParse(subItem.ToString(), out _))
                {
                    exceptions.Add(new FailedOperationException("The given subItem ID could not be parsed into a GUID"));
                }
            }
        }

        // ! Validate the GUIDs to remove
        if (subItemsToRemove != null)
        {
            foreach (var subItem in subItemsToRemove)
            {
                if (!Guid.TryParse(subItem.ToString(), out _))
                {
                    exceptions.Add(new FailedOperationException("The given subItem ID could not be parsed into a GUID"));
                }
            }
        }

        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray())
            : Result.Success();
    }
}