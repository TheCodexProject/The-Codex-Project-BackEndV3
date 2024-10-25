using domain.exceptions;
using domain.models.user;
using domain.models.workspace;
using OperationResult;

namespace domain.models.organization;

/// <summary>
/// This class handles validation all properties relating to the <see cref="Organization"/> class.
/// </summary>
public static class OrganizationPropertyValidator
{
    /// <summary>
    /// Validates the name of an organization.
    /// </summary>
    /// <param name="name">Name to be checked.</param>
    /// <returns></returns>
    public static Result<string> ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<string>.Failure(new InvalidArgumentException("Organization name cannot be empty, please provide a name."));
        }

        return name.Length switch
        {
            < 2 => Result<string>.Failure(new InvalidArgumentException("Organization name is too short, please provide a name with at least 2 characters.")),
            > 100 => Result<string>.Failure(new InvalidArgumentException("Organization name is too long, please provide a name with at most 100 characters.")),
            _ => Result<string>.Success(name)
        };
    }

    /// <summary>
    /// Validates the addition of an owner to an organization.
    /// </summary>
    /// <param name="member"></param>
    /// <param name="members"></param>
    /// <returns></returns>
    public static Result ValidateAddMember(User? member, List<User> members)
    {
        // ? Is the member null or empty?
        if (member == null)
            return Result.Failure(new InvalidArgumentException("The provided member is invalid. Member cannot be null."));

        // ? Does the member already exist in the list?
        return members.Contains(member) ?
            Result.Failure(new InvalidArgumentException("The provided member already exists in the list."))
            : Result.Success();
    }

    /// <summary>
    /// Validates the removal of an owner from an organization.
    /// </summary>
    /// <param name="member"></param>
    /// <param name="members"></param>
    /// <returns></returns>
    public static Result ValidateRemoveMember(User? member, List<User> members)
    {
        // ? Is the member null or empty?
        if (member == null)
            return Result.Failure(new InvalidArgumentException("The provided member is invalid. Member cannot be null."));

        // ? Does the member exist in the list?
        return members.Contains(member) ?
            Result.Success()
            : Result.Failure(new InvalidArgumentException("The provided member does not exist in the list."));
    }

    public static Result ValidateAddWorkspace(Workspace? workspace, List<Workspace> workspaces)
    {
        // ? Is the workspace null or empty?
        if (workspace == null)
            return Result.Failure(new InvalidArgumentException("The provided workspace is invalid. Workspace cannot be null."));

        // ? Does the workspace already exist in the list?
        return workspaces.Contains(workspace) ?
            Result.Failure(new InvalidArgumentException("The provided workspace already exists in the list."))
            : Result.Success();
    }

    public static Result ValidateRemoveWorkspace(Workspace? workspace, List<Workspace> workspaces)
    {
        // ? Is the workspace null or empty?
        if (workspace == null)
            return Result.Failure(new InvalidArgumentException("The provided workspace is invalid. Workspace cannot be null."));

        // ? Does the workspace exist in the list?
        return workspaces.Contains(workspace)
            ? Result.Success()
            : Result.Failure(new InvalidArgumentException("The provided workspace does not exist in the list."));
    }

}