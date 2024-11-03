using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using domain.models.project;
using domain.models.projectActivity.value;
using domain.models.workItem;
using OperationResult;

namespace domain.models.projectActivity;

/// <summary>
/// A project activity that could either be a milestone or iteration/sprint.
/// </summary>
public class ProjectActivity
{
    // # METADATA #
    /// <summary>
    /// The ID of the project phase.
    /// </summary>
    [Key]
    public Guid Id { get; private set; }

    /// <summary>
    /// The time the project phase was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    /// <summary>
    /// The user who created the project phase.
    /// </summary>
    public string CreatedBy { get; private set; }

    /// <summary>
    /// The last time the project phase was updated. (Default is DateTime.MinValue, if the project phase has never been updated.)
    /// </summary>
    public DateTime UpdatedAt { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// The user who last updated the project phase.
    /// </summary>
    public string UpdatedBy { get; private set; } = string.Empty;

    // # PROPERTIES #
    /// <summary>
    /// Title of the project phase.
    /// </summary>
    [Required]
    [MaxLength(75)]
    [MinLength(2)]
    public string Title { get; private set; }

    /// <summary>
    /// Description of the project phase.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; private set; } = string.Empty;

    /// <summary>
    /// The work items that are part of the project phase.
    /// </summary>
    private List<WorkItem> _workItems { get; } = new List<WorkItem>();

    [NotMapped]
    public ReadOnlyCollection<WorkItem> WorkItems => _workItems.AsReadOnly();

    /// <summary>
    /// The type of project phase (Milestone or Iteration)
    /// </summary>
    public ProjectActivityType Type { get; private set; } = ProjectActivityType.None;

    /// <summary>
    /// A reference to the project that the project phase belongs to.
    /// </summary>
    [Required]
    public Guid ProjectId { get; }

    public Project Project { get; private set; }

    // # CONSTRUCTORS #

    // NOTE: EF Core requires a parameterless constructor.
    private ProjectActivity() { }

    private ProjectActivity(Project project, string title, ProjectActivityType type)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        CreatedBy = "TODO: Implement";

        ProjectId = project.Id;
        Title = title;
        Type = type;
    }

    public static Result<ProjectActivity> Create(Project projectId, string title, ProjectActivityType type)
    {
        // ! Validate the project phase's input here.
        var validationResult = Validate( title, type);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            // ! Return the errors
            return Result<ProjectActivity>.Failure(validationResult.Errors.ToArray());

        var projectActivity = new ProjectActivity(projectId, title, type);

        // ! Try to add the project phase to the project
        var addProjectPhaseResult = projectId.AddActivity(projectActivity);

        // ? Was the project phase added successfully?
        if (addProjectPhaseResult.IsFailure)
            // ! Return the errors
            return Result<ProjectActivity>.Failure(addProjectPhaseResult.Errors.ToArray());

        // * Return a new project phase.
        return projectActivity;
    }

    private static Result Validate(string title, ProjectActivityType type)
    {
        // * List of if there occur any errors during validation
        List<Exception> errors = [];

        // ! Validate the title
        var titleValidation = ProjectActivityPropertyValidator.ValidateTitle(title);

        // ? Were there any errors during validation of the title?
        if (titleValidation.IsFailure)
        {
            errors.AddRange(titleValidation.Errors);
        }

        // ! Validate the type
        var typeValidation = ProjectActivityPropertyValidator.ValidateType(type);

        // ? Were there any errors during validation of the type?
        if (typeValidation.IsFailure)
        {
            errors.AddRange(typeValidation.Errors);
        }

        return errors.Count != 0
            ? Result.Failure(errors.ToArray()) // ! Return the errors
            : Result.Success(); // * Return a successful validation
    }

    // # METHODS #

    public Result UpdateTitle(string title)
    {
        // ! Validate the title
        var validationResult = ProjectActivityPropertyValidator.ValidateTitle(title);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
        {
            return Result.Failure(validationResult.Errors.ToArray());
        }

        // * Update the title
        Title = title;

        return Result.Success();
    }

    public Result UpdateDescription(string description)
    {
        // ! Validate the description
        var validationResult = ProjectActivityPropertyValidator.ValidateDescription(description);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
        {
            return Result.Failure(validationResult.Errors.ToArray());
        }

        // * Update the description
        Description = description;

        return Result.Success();
    }

    public Result UpdateType(ProjectActivityType type)
    {
        // ! Validate the type
        var validationResult = ProjectActivityPropertyValidator.ValidateType(type);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
        {
            return Result.Failure(validationResult.Errors.ToArray());
        }

        // * Update the type
        Type = type;

        return Result.Success();
    }

    public Result AddWorkItem(WorkItem workItem)
    {
        // ! Validate the work item
        var validationResult = ProjectActivityPropertyValidator.ValidateAddWorkItem(workItem, _workItems);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
            // ! Return the errors
            return Result.Failure(validationResult.Errors.ToArray());

        // * Add the activity to the work item
        var addActivity = workItem.AddActivity(this);

        // ? Was the activity added successfully?
        if (addActivity.IsFailure)
            // ! Return the errors
            return Result.Failure(addActivity.Errors.ToArray());

        // * Add the work item
        _workItems.Add(workItem);

        return Result.Success();
    }

    public Result RemoveWorkItem(WorkItem workItem)
    {
        // ! Validate the work item
        var validationResult = ProjectActivityPropertyValidator.ValidateRemoveWorkItem(workItem, _workItems);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
            // ! Return the errors
            return Result.Failure(validationResult.Errors.ToArray());


        // * Remove the activity from the work item
        var removeActivity = workItem.RemoveActivity(this);

        // ? Was the activity removed successfully?
        if (removeActivity.IsFailure)
            // ! Return the errors
            return Result.Failure(removeActivity.Errors.ToArray());

        // * Remove the work item
        _workItems.Remove(workItem);

        return Result.Success();
    }
}