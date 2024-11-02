using System.ComponentModel.DataAnnotations;
using domain.interfaces;
using domain.models.organization;
using domain.models.project;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.user;
using OperationResult;

namespace domain.models.workspace;

/// <summary>
/// Represents a workspace.
/// </summary>
public class Workspace : IResourceOwner
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
    public Organization Owner { get; private set; }

    [Required]
    [MaxLength(100)]
    [MinLength(3)]
    public string Title { get; private set; }

    public List<User> Contacts { get; private set; } = new List<User>();

    public List<Project> Projects { get; private set; } = new List<Project>();

    public List<Resource> Resources { get; private set; } = new List<Resource>();

    // # CONSTRUCTORS #

    // NOTE: EF Core requires a parameterless constructor.
    private Workspace() { }

    private Workspace(Organization owner, string title)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        CreatedBy = owner.Owner.Email;

        Owner = owner;
        owner.AddWorkspace(this); // NOTE: This is a bidirectional relationship (Now the organization knows about the workspace.)
        Title = title;
        Contacts = [];
    }

    public static Result<Workspace> Create(Organization owner, string title)
    {
        // ! Validate the workspace's input here.
        var validationResult = Validate(title);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
            return Result<Workspace>.Failure(validationResult.Errors.ToArray());

        return Result<Workspace>.Success(new Workspace(owner, title));
    }

    private static Result Validate(string title)
    {
        // ! Validate the title
        var titleValidation = WorkspacePropertyValidator.ValidateTitle(title);

        // ? Is the validation a failure?
        if (titleValidation.IsFailure)
            return Result.Failure(titleValidation.Errors.ToArray());

        return Result.Success();
    }

    // # METHODS #
    public Result UpdateTitle(string title)
    {
        // ? Validate the input.
        var result = WorkspacePropertyValidator.ValidateTitle(title);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Title = title;
        return Result.Success();
    }

    public Result AddContact(User contact)
    {
        // ? Validate the input.
        var result = WorkspacePropertyValidator.ValidateAddContact(contact, Contacts);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Contacts.Add(contact);
        return Result.Success();
    }

    public Result RemoveContact(User contact)
    {
        // ? Validate the input.
        var result = WorkspacePropertyValidator.ValidateRemoveContact(contact, Contacts);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Contacts.Remove(contact);
        return Result.Success();
    }

    public Result AddProject(Project project)
    {
        // ? Validate the input.
        var result = WorkspacePropertyValidator.ValidateAddProject(project, Projects);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Projects.Add(project);
        return Result.Success();
    }

    public Result RemoveProject(Project project)
    {
        // ? Validate the input.
        var result = WorkspacePropertyValidator.ValidateRemoveProject(project, Projects);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Projects.Remove(project);
        return Result.Success();
    }

    public Result AddResource(Resource resource)
    {
        // * Add the resource to the workspace.
        var addValidationResult = WorkspacePropertyValidator.ValidateAddResource(resource, Resources);

        // ? Is the add validation a failure?
        if (addValidationResult.IsFailure)
        {
            return Result.Failure(addValidationResult.Errors.ToArray());
        }

        Resources.Add(resource);
        return Result.Success();
    }
    
    public Result RemoveResource(Resource resource)
    {
        // ? Validate the input.
        var result = WorkspacePropertyValidator.ValidateRemoveResource(resource, Resources);

        // ? Is the validation a failure?
        if (result.IsFailure)
            return Result.Failure(result.Errors.ToArray());

        Resources.Remove(resource);
        return Result.Success();
    }
}