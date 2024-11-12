using System.ComponentModel.DataAnnotations;
using domain.interfaces;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.user;
using domain.models.workspace;
using OperationResult;

namespace domain.models.organization;

/// <summary>
/// Represents an organization.
/// </summary>
public class Organization : IResourceOwner
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
    /// The name of the organization.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [MinLength(2)]
    public string Name { get; private set; }

    /// <summary>
    /// The owner of the organization.
    /// </summary>
    [Required]
    public User Owner { get; private set; }

    // NOTE: EF Core requires a foreign key for the owner.
    private Guid _ownerId;

    /// <summary>
    /// Members of the organization.
    /// </summary>
    public List<User> Members { get; private set; } = new List<User>();

    /// <summary>
    /// Workspaces within the organization.
    /// </summary>
    public List<Workspace> Workspaces { get; private set; } = new List<Workspace>();

    /// <summary>
    /// Resources on the organization-level.
    /// </summary>
    public List<Resource> Resources { get; private set; } = new List<Resource>();
    
    // # CONSTRUCTORS #

    // NOTE: EF Core requires a parameterless constructor.
    private Organization() {}

    private Organization(string name, User owner)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        CreatedBy = owner.Email;

        Name = name;
        Owner = owner;
        Members = [];
    }

    public static Result<Organization> Create(string name, User owner)
    {
        // ! Validate the organization's input here.
        var validationResult = Validate(name);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
            return Result<Organization>.Failure(validationResult.Errors.ToArray());

        return Result<Organization>.Success(new Organization(name, owner));
    }

    private static Result Validate(string name)
    {
        // ! Validate the name.
        var nameValidation = OrganizationPropertyValidator.ValidateName(name);

        // ? Is the name a failure?
        if (nameValidation.IsFailure)
            return Result.Failure(nameValidation.Errors.ToArray());

        return Result.Success();
    }

    // # METHODS #

    /// <summary>
    /// Updates the name of the organization.
    /// </summary>
    /// <param name="name">The new name</param>
    /// <returns>A <see cref="Result"/> indicating if the update was successful or a failure.
    /// If it is a failure, there will be a list of exceptions to check.</returns>
    public Result UpdateName(string name)
    {
        // ! Validate the name.
        var result = OrganizationPropertyValidator.ValidateName(name);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            // ! Return the failure.
            return Result.Failure(result.Errors.ToArray());
        }

        Name = name;
        return Result.Success();
    }

    /// <summary>
    /// Adds a member to the organization.
    /// </summary>
    /// <param name="member">The member to add.</param>
    /// <returns>A <see cref="Result"/> indicating if the addition was successful or a failure.
    /// If it is a failure, there will be a list of exceptions to check.</returns>
    public Result AddMember(User member)
    {
        // ! Validate the member.
        var result = OrganizationPropertyValidator.ValidateAddMember(member, Members);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            // ! Return the failure.
            return Result.Failure(result.Errors.ToArray());
        }

        Members.Add(member);
        member.JoinOrganization(this);
        return Result.Success();
    }

    /// <summary>
    /// Removes a member from the organization.
    /// </summary>
    /// <param name="member">The member to remove.</param>
    /// <returns>A <see cref="Result"/> indicating if the removal was successful or a failure.
    /// If it is a failure, there will be a list of exceptions to check.</returns>
    public Result RemoveMember(User member)
    {
        // ! Validate the member.
        var result = OrganizationPropertyValidator.ValidateRemoveMember(member, Members);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            // ! Return the failure.
            return Result.Failure(result.Errors.ToArray());
        }

        Members.Remove(member);
        member.LeaveOrganization(this);
        return Result.Success();
    }

    /// <summary>
    /// Adds a workspace from another organization to this organization.
    /// This is useful for when you want to move a workspace between organizations.
    /// (e.g., a user leaves an organization and wants to take their workspace with them or an acquisition)
    /// </summary>
    /// <param name="workspace">The workspace to add.</param>
    /// <returns>A <see cref="Result"/> indicating if the addition was successful or a failure.
    /// If it is a failure, there will be a list of exceptions to check.</returns>
    public Result AddWorkspace(Workspace workspace)
    {
        // ! Validate the workspace.
        var result = OrganizationPropertyValidator.ValidateAddWorkspace(workspace, Workspaces);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            // ! Return the failure.
            return Result.Failure(result.Errors.ToArray());
        }

        Workspaces.Add(workspace);
        return Result.Success();
    }

    /// <summary>
    /// Removes a workspace from the organization.
    /// This is decouples the workspace from the organization without deleting the workspace itself.
    /// </summary>
    /// <param name="workspace">The workspace to remove.</param>
    /// <returns>A <see cref="Result"/> indicating if the removal was successful or a failure.
    /// If it is a failure, there will be a list of exceptions to check.</returns>
    public Result RemoveWorkspace(Workspace workspace)
    {
        // ! Validate the workspace.
        var result = OrganizationPropertyValidator.ValidateRemoveWorkspace(workspace, Workspaces);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            // ! Return the failure.
            return Result.Failure(result.Errors.ToArray());
        }

        Workspaces.Remove(workspace);
        return Result.Success();
    }

    /// <summary>
    /// Adds a resource for use within the organization.
    /// </summary>
    /// <param name="resource">The resource to add.</param>
    /// <returns>A <see cref="Result"/> indicating if the addition was successful or a failure.
    /// If it is a failure, there will be a list of exceptions to check.</returns>
    public Result AddResource(Resource resource)
    {
        // * Add the resource to the organization.
        var addValidationResult = OrganizationPropertyValidator.ValidateAddResource(resource, Resources);
        
        // ? Is the result a failure?
        if (addValidationResult.IsFailure)
        {
            // ! Return the failure.
            return Result.Failure(addValidationResult.Errors.ToArray());
        }
        
        Resources.Add(resource);
        return Result.Success();
    }

    /// <summary>
    /// Removes a resource from the organization.
    /// </summary>
    /// <param name="resource">The resource to remove.</param>
    /// <returns>A <see cref="Result"/> indicating if the removal was successful or a failure.
    /// If it is a failure, there will be a list of exceptions to check.</returns>
    public Result RemoveResource(Resource resource)
    {
        // ! Validate the resource.
        var result = OrganizationPropertyValidator.ValidateRemoveResource(resource, Resources);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            // ! Return the failure.
            return Result.Failure(result.Errors.ToArray());
        }

        Resources.Remove(resource);
        return Result.Success();
    }
}