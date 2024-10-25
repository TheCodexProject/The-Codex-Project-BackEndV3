using System.ComponentModel.DataAnnotations;
using domain.models.organization;
using OperationResult;

namespace domain.models.user;

/// <summary>
/// This class represents a user within the system.
/// </summary>
public class User
{
    // # METADATA #

    /// <summary>
    /// The ID of the user.
    /// </summary>
    [Key]
    public Guid Id { get; private set; }

    /// <summary>
    /// When the user was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// The last time the user was updated. (Default is DateTime.MinValue, if the user has never been updated.)
    /// </summary>
    public DateTime UpdatedAt { get; private set; } = DateTime.MinValue;

    // # PROPERTIES #

    [Required]
    [MaxLength(50)]
    [MinLength(2)]
    public string FirstName { get; private set; }

    [Required]
    [MaxLength(60)]
    [MinLength(2)]
    public string LastName { get; private set; }

    [Required]
    [MaxLength(60)]
    public string Email { get; private set; }

    public List<Organization> OwnedOrganizations { get; private set; } = new List<Organization>();
    public List<Organization> Memberships { get; private set; } = new List<Organization>();

    // # CONSTRUCTORS #

    // NOTE: EF Core requires a parameterless constructor.
    private User() { }

    private User(string firstName, string lastName, string email)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<User> Create(string firstName, string lastName, string email)
    {
        // ! Validate the user's input here.
        var validationResult = Validate(firstName, lastName, email);

        // ? Is the validation a failure?
        if (validationResult.IsFailure)
            return Result<User>.Failure(validationResult.Errors.ToArray());

        return new User(firstName, lastName, email);
    }

    private static Result Validate(string firstName, string lastName, string email)
    {
        // ! Validate the first name.
        var firstNameResult = UserPropertyValidator.ValidateFirstName(firstName);

        // ? Is the result a failure?
        if (firstNameResult.IsFailure)
        {
            return Result.Failure(firstNameResult.Errors.ToArray());
        }

        // ! Validate the last name.
        var lastNameResult = UserPropertyValidator.ValidateLastName(lastName);

        // ? Is the result a failure?
        if (lastNameResult.IsFailure)
        {
            return Result.Failure(lastNameResult.Errors.ToArray());
        }

        // ! Validate the email.
        var emailResult = UserPropertyValidator.ValidateEmail(email);

        // ? Is the result a failure?
        if (emailResult.IsFailure)
        {
            return Result.Failure(emailResult.Errors.ToArray());
        }

        return Result.Success();
    }

    // # METHODS #

    public Result UpdateFirstName(string firstName)
    {
        // ! Validate the first name.
        var result = UserPropertyValidator.ValidateFirstName(firstName);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors.ToArray());
        }

        // * Update the first name.
        FirstName = firstName;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result UpdateLastName(string lastName)
    {
        // ! Validate the last name.
        var result = UserPropertyValidator.ValidateLastName(lastName);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors.ToArray());
        }

        // * Update the last name.
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result UpdateEmail(string email)
    {
        // ! Validate the email.
        var result = UserPropertyValidator.ValidateEmail(email);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors.ToArray());
        }

        // * Update the email.
        Email = email;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result JoinOrganization(Organization organization)
    {
        // ? Validate the organization.
        var result = UserPropertyValidator.ValidateAddOrganization(organization, Memberships);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            // ! Return the failure.
            return Result.Failure(result.Errors.ToArray());
        }

        // * Add the organization.
        Memberships.Add(organization);
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result LeaveOrganization(Organization organization)
    {
        // ? Validate the organization.
        var result = UserPropertyValidator.ValidateRemoveOrganization(organization, Memberships);

        // ? Is the result a failure?
        if (result.IsFailure)
        {
            // ! Return the failure.
            return Result.Failure(result.Errors.ToArray());
        }

        // * Remove the organization.
        Memberships.Remove(organization);
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}