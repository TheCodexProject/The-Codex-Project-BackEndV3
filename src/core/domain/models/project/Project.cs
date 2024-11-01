using System.ComponentModel.DataAnnotations;
using domain.interfaces;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.workItem;
using domain.models.workspace;
using domain.shared;
using OperationResult;

namespace domain.models.project;

public class Project : IResourceOwner
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
    public Workspace Workspace { get; private set; }

    [Required]
    [MaxLength(100)]
    [MinLength(3)]
    public string Title { get; private set; }

    /// <summary>
    /// Description of the project. (Default: Empty)
    /// </summary>
    [MaxLength(500)]
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Status of the project. (Default: None)
    /// </summary>
    public Status Status { get; private set; } = Status.None;

    /// <summary>
    /// Priority of the project. (Default: None)
    /// </summary>
    public Priority Priority { get; private set; } = Priority.None;

    /// <summary>
    /// Start of the project. (Default: DateTime.MinValue, if not set)
    /// </summary>
    public DateTime Start { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// End of the project. (Default: DateTime.MinValue, if not set)
    /// </summary>
    public DateTime End { get; private set; } = DateTime.MinValue;

    public List<WorkItem> Tasks { get; private set; } = new List<WorkItem>();

    public List<Resource> Resources { get; private set; } = new List<Resource>();

    // # CONSTRUCTORS #

    // NOTE: EF Core requires a parameterless constructor.
    private Project() { }

    private Project(Workspace workspace, string title)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        CreatedBy = workspace.Owner.Owner.Email;

        Workspace = workspace;
        workspace.AddProject(this);
        Title = title;
    }

    public static Result<Project> Create(Workspace workspace, string title)
    {
        // ! Validate the project's input here.
        var validationResult = Validate(title);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
            return Result<Project>.Failure(validationResult.Errors.ToArray());

        return Result<Project>.Success(new Project(workspace, title));
    }

    private static Result Validate(string title)
    {
        // ! Validate the title
        var titleValidation = ProjectPropertyValidator.ValidateTitle(title);

        // ? Is the validation a failure?
        if (titleValidation.IsFailure)
            return Result.Failure(titleValidation.Errors.ToArray());

        return Result.Success();
    }

    // # METHODS #
    public Result UpdateTitle(string title)
    {
        // ? Validate the input.
        var result = ProjectPropertyValidator.ValidateTitle(title);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Title = title;
        return Result.Success();
    }

    public Result UpdateDescription(string description)
    {
        // ? Validate the input.
        var result = ProjectPropertyValidator.ValidateDescription(description);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Description = description;
        return Result.Success();
    }

    public Result UpdateStatus(Status status)
    {
        // ? Validate the input.
        var result = ProjectPropertyValidator.ValidateStatus(status);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Status = status;
        return Result.Success();
    }

    public Result UpdatePriority(Priority priority)
    {
        // ? Validate the input.
        var result = ProjectPropertyValidator.ValidatePriority(priority);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Priority = priority;
        return Result.Success();
    }

    public Result UpdateTimeRange(DateTime start, DateTime end)
    {
        // ? Validate the input.
        var result = ProjectPropertyValidator.ValidateTimeRange(start, end);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Start = start;
        End = end;
        return Result.Success();
    }

    public Result AddTask(WorkItem task)
    {
        // ? Validate the input.
        var result = ProjectPropertyValidator.ValidateAddWorkItem(task, Tasks);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Tasks.Add(task);
        return Result.Success();
    }

    public Result RemoveTask(WorkItem task)
    {
        // ? Validate the input.
        var result = ProjectPropertyValidator.ValidateRemoveWorkItem(task, Tasks);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Tasks.Remove(task);
        return Result.Success();
    }

    public Result AddResource(string title, string url)
    {
        // * Try to create a new resource.
        var resourceResult = Resource.Create(title, url, Id, ResourceLevel.Project);
        
        // ? Is the resource creation a failure?
        if (resourceResult.IsFailure)
            return Result.Failure(resourceResult.Errors.ToArray());
        
        // * Add the resource to the project.
        var addValidationResult = ProjectPropertyValidator.ValidateAddResource(resourceResult.Value, Resources);
        
        // ? Is the validation a failure?
        if (addValidationResult.IsFailure)
            return Result.Failure(addValidationResult.Errors.ToArray());
        
        Resources.Add(resourceResult.Value);
        
        return Result.Success();
    }

    public Result RemoveResource(Resource resource)
    {
        // ? Validate the input.
        var result = ProjectPropertyValidator.ValidateRemoveResource(resource, Resources);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Resources.Remove(resource);
        return Result.Success();
    }
}