using domain.models.user;
using OperationResult;

namespace application.appEntry.commands.user;

public class CreateUserCommand
{
    public Guid Id { get; set; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }

    private CreateUserCommand(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<CreateUserCommand> Create(string firstName, string lastName, string email)
    {
        // ! Validate the user's input
        var validationResult = Validate(firstName, lastName, email);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<CreateUserCommand>.Failure(validationResult.Errors.ToArray());

        // * Return the newly created command.
        return new CreateUserCommand(firstName, lastName, email);
    }

    private static Result Validate(string firstName, string lastName, string email)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = new List<Exception>();

        // ! Validate the first name
        var firstNameValidation = UserPropertyValidator.ValidateFirstName(firstName);

        // ? Did the first name validation fail?
        if (firstNameValidation.IsFailure)
            exceptions.AddRange(firstNameValidation.Errors);

        // ! Validate the last name
        var lastNameValidation = UserPropertyValidator.ValidateLastName(lastName);

        // ? Did the last name validation fail?
        if (lastNameValidation.IsFailure)
            exceptions.AddRange(lastNameValidation.Errors);

        // ! Validate the email
        var emailValidation = UserPropertyValidator.ValidateEmail(email);

        // ? Did the email validation fail?
        if (emailValidation.IsFailure)
            exceptions.AddRange(emailValidation.Errors);

        // ? Were there any exceptions?
        return exceptions.Count != 0
            ? Result.Failure(exceptions.ToArray()) // * Yes: Return the exceptions
            : Result.Success(); // * No: Return success
    }



}