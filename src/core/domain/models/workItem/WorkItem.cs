using System.ComponentModel.DataAnnotations;
using domain.models.project;
using domain.models.user;
using domain.models.workItem.values;
using domain.shared;
using OperationResult;

namespace domain.models.workItem;

public class WorkItem
{
    // # METADATA #
    [Key]
    public Guid Id { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; }

    public DateTime UpdatedAt { get; private set; } = DateTime.MinValue;
    public string UpdatedBy { get; private set; } = string.Empty;

    // # PROPERTIES #
    /// <summary>
    /// The project that the work item belongs to. (Required)
    /// </summary>
    [Required]
    public Project Project { get; private set; }

    /// <summary>
    /// The title of the work item. (Required)
    /// </summary>
    [Required]
    [MaxLength(75)]
    [MinLength(3)]
    public string Title { get; private set; }

    /// <summary>
    /// The description of the work item. (Default: Empty)
    /// </summary>
    [MaxLength(500)]
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Status of the work item. (Default: None)
    /// </summary>
    public Status Status { get; private set; } = Status.None;

    /// <summary>
    /// Priority of the work item. (Default: None)
    /// </summary>
    public Priority Priority { get; private set; } = Priority.None;

    /// <summary>
    /// Type of the work item. (Default: Task)
    /// </summary>
    public ItemType Type { get; private set; } = ItemType.None;

    /// <summary>
    /// The user that the work item is assigned to. (Default: None)
    /// </summary>
    public User? AssignedTo { get; private set; } = null;

    public WorkItem? Parent { get; private set; } = null;

    /// <summary>
    /// Subitems that are part of the work item.
    /// </summary>
    public List<WorkItem> Subitems { get; private set; } = new List<WorkItem>();

    // # CONSTRUCTORS #

    // NOTE: EF Core requires a parameterless constructor.
    private WorkItem() {}

    private WorkItem(Project project, string title)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        CreatedBy = "TODO: Implement";

        Project = project;
        Title = title;
        Subitems = [];
    }

    public static Result<WorkItem> Create(Project project, string title)
    {
        // ! Validate the work item's input here.
        var validationResult = Validate(title);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
            return Result<WorkItem>.Failure(validationResult.Errors.ToArray());

        return Result<WorkItem>.Success(new WorkItem(project, title));
    }

    private static Result Validate(string title)
    {
        // ! Validate the title
        var titleValidation = WorkItemPropertyValidator.ValidateTitle(title);

        // ? Is the validation a failure?
        if (titleValidation.IsFailure)
            return Result.Failure(titleValidation.Errors.ToArray());

        return Result.Success();
    }

    // # METHODS #
    public Result UpdateTitle(string title)
    {
        // ? Validate the input.
        var result = WorkItemPropertyValidator.ValidateTitle(title);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Title = title;
        return Result.Success();
    }

    public Result UpdateDescription(string description)
    {
        // ? Validate the input.
        var result = WorkItemPropertyValidator.ValidateDescription(description);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Description = description;
        return Result.Success();
    }

    public Result UpdateStatus(Status status)
    {
        // ? Validate the input.
        var result = WorkItemPropertyValidator.ValidateStatus(status);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Status = status;
        return Result.Success();
    }

    public Result UpdatePriority(Priority priority)
    {
        // ? Validate the input.
        var result = WorkItemPropertyValidator.ValidatePriority(priority);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Priority = priority;
        return Result.Success();
    }

    public Result UpdateType(ItemType type)
    {
        Type = type;
        return Result.Success();
    }

    public Result UpdateAssignedTo(User? assignedTo)
    {
        // ? Validate the input.
        var result = WorkItemPropertyValidator.ValidateAssignedTo(assignedTo);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        AssignedTo = assignedTo;
        return Result.Success();
    }

    public Result AddSubitem(WorkItem subitem)
    {
        // ? Validate the input.
        var result = WorkItemPropertyValidator.ValidateAddSubitem(subitem, Subitems);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        subitem.UpdateParent(this);
        Subitems.Add(subitem);
        return Result.Success();
    }

    public Result RemoveSubitem(WorkItem subitem)
    {
        // ? Validate the input.
        var result = WorkItemPropertyValidator.ValidateRemoveSubitem(subitem, Subitems);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        subitem.RemoveParent();
        Subitems.Remove(subitem);
        return Result.Success();
    }

    private Result UpdateParent(WorkItem parent)
    {
        Parent = parent;
        return Result.Success();
    }
    private Result RemoveParent()
    {
        Parent = null;
        return Result.Success();
    }

    // TODO: TO BE EXTENDED
}