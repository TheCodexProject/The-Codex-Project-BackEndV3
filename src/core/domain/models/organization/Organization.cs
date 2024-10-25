using System.ComponentModel.DataAnnotations;
using domain.models.user;
using domain.models.workspace;
using OperationResult;

namespace domain.models.organization;

/// <summary>
/// Represents an organization.
/// </summary>
public class Organization
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
    public List<User> Members { get; private set; }

    public List<Workspace> Workspaces { get; private set; }

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
}