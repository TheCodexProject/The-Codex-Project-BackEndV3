using System.Text.RegularExpressions;
using domain.exceptions;
using OperationResult;

namespace domain.models.user;

/// <summary>
/// This class handles validation all properties relating to the <see cref="User"/> class.
/// </summary>
public static class UserPropertyValidator
{
    /// <summary>
    /// Validates the first name of a user.
    /// </summary>
    /// <param name="firstName">First name to be validated.</param>
    /// <returns>A <see cref="Result"/> indicating if first name didn't pass the validation rules.</returns>
    public static Result<string> ValidateFirstName(string firstName)
    {
        // ? Is the first name empty?
        if (string.IsNullOrWhiteSpace(firstName))
            return Result<string>.Failure(new InvalidArgumentException("First name cannot be empty, please provide a first name."));

        // ? Does the first name contain any non-alphabetic characters?
        if (!Regex.IsMatch(firstName, @"^[a-zA-Z\s]+$"))
            return Result<string>.Failure(new InvalidArgumentException("First name can only contain alphabetic characters and spaces."));

        // ? Does the first name contain extra spaces?
        if (firstName.Trim() != firstName || firstName.Contains("  "))
            return Result<string>.Failure(new InvalidArgumentException("First name cannot contain leading, trailing, or multiple consecutive spaces."));

        // ? Is the first name too short?
        if (firstName.Length < 2)
            return Result<string>.Failure(new InvalidArgumentException("First name is too short, please provide a first name with at least 2 characters."));

        // ? Is the first name too long?
        if (firstName.Length > 50)
            return Result<string>.Failure(new InvalidArgumentException("First name is too long, please provide a first name with at most 50 characters."));

        return Result<string>.Success(firstName);
    }

    /// <summary>
    /// Validates the last name of a user.
    /// </summary>
    /// <param name="lastName">Last name to be validated.</param>
    /// <returns>A <see cref="Result"/> indicating if last name didn't pass the validation rules. </returns>
    public static Result<string> ValidateLastName(string lastName)
    {
        // ? Is the last name empty?
        if (string.IsNullOrWhiteSpace(lastName))
            return Result<string>.Failure(new InvalidArgumentException("Last name cannot be empty, please provide a last name."));

        // ? Does the last name contain any non-alphabetic characters?
        if (!Regex.IsMatch(lastName, @"^[a-zA-Z\s]+$"))
            return Result<string>.Failure(new InvalidArgumentException("Last name can only contain alphabetic characters and spaces."));

        // ? Does the last name contain extra spaces?
        if (lastName.Trim() != lastName || lastName.Contains("  "))
            return Result<string>.Failure(new InvalidArgumentException("Last name cannot contain leading, trailing, or multiple consecutive spaces."));

        // ? Is the last name too short?
        if (lastName.Length < 2)
            return Result<string>.Failure(new InvalidArgumentException("Last name is too short, please provide a last name with at least 2 characters."));

        // ? Is the last name too long?
        if (lastName.Length > 60)
            return Result<string>.Failure(new InvalidArgumentException("Last name is too long, please provide a last name with at most 60 characters."));

        return Result<string>.Success(lastName);
    }

    /// <summary>
    /// Validates the email of a user.
    /// </summary>
    /// <param name="email">Email to be checked</param>
    /// <returns>A <see cref="Result"/> indicating if email didn't pass the validation rules.</returns>
    public static Result<string> ValidateEmail(string email)
    {
        // ? Is the email empty?
        if(string.IsNullOrWhiteSpace(email))
            return Result<string>.Failure(new InvalidArgumentException("Email cannot be empty, please provide an email."));

        if(!EmailFormatCheck(email))
            return Result<string>.Failure(new InvalidArgumentException("Email is invalid, please provide a valid email."));

        return Result<string>.Success(email);
    }

    /// <summary>
    /// Checks if the email is in the correct format.
    /// </summary>
    /// <param name="value">Email to be checked.</param>
    /// <returns>a <see cref="bool"/> indicating if the email is in the correct format.</returns>
    private static bool EmailFormatCheck(string value)
    {
        const string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // ? Is the email in the correct format?
        if (!Regex.IsMatch(value, pattern))
        {
            return false;
        }

        // ? Is the email too long?
        var parts = value.Split('@');
        if (parts.Length != 2)
        {
            return false;
        }


        var localPart = parts[0];
        var domainPart = parts[1];

        // ? Is the local part or domain part too long?
        if (localPart.Length > 30 || value.Length > 254)
        {
            return false;
        }

        // ? Does the local part or domain part contain ".."?
        if (localPart.Contains("..") || domainPart.Contains(".."))
        {
            return false;
        }

        // ? Does the local part start or end with a period?
        if (localPart.StartsWith('.') || localPart.EndsWith('.'))
        {
            return false;
        }

        // ? Is the domain part too long?
        var domainParts = domainPart.Split('.');

        return !domainParts.Any(part => part.Length > 63);
    }
}