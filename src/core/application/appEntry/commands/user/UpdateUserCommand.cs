using domain.exceptions;
using domain.models.user;
using OperationResult;

namespace application.appEntry.commands.user;

public class UpdateUserCommand
{
    public Guid Id { get; set; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public string? Email { get; }

    public User? User { get; set; }

    private UpdateUserCommand(Guid id, string? firstName, string? lastName, string? email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<UpdateUserCommand> Create(string? id, string? firstName, string? lastName, string? email)
    {
        var validationResult = Validate(id, firstName, lastName, email);

        if (validationResult.IsFailure)
            return Result<UpdateUserCommand>.Failure(validationResult.Errors.ToArray());

        return new UpdateUserCommand(new Guid(id), firstName, lastName, email);
    }

    private static Result Validate(string id, string? firstName, string? lastName, string? email)
    {
        // * List for exceptions during validation
        List<Exception> exceptions = [];

        if (!Guid.TryParse(id, out var parsedId))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        if (firstName is not null)
        {
            var firstNameValidation = UserPropertyValidator.ValidateFirstName(firstName);

            if (firstNameValidation.IsFailure)
                exceptions.AddRange(firstNameValidation.Errors);
        }

        if (lastName is not null)
        {
            var lastNameValidation = UserPropertyValidator.ValidateLastName(lastName);

            if (lastNameValidation.IsFailure)
                exceptions.AddRange(lastNameValidation.Errors);
        }

        if (email is not null)
        {
            var emailValidation = UserPropertyValidator.ValidateEmail(email);

            if (emailValidation.IsFailure)
                exceptions.AddRange(emailValidation.Errors);
        }

        return (exceptions.Count != 0)
            ? Result.Failure(exceptions.ToArray())
            : Result.Success();
    }


}