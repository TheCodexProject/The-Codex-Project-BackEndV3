using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.organization;
using OperationResult;

namespace application.features.organization;

public class CreateOrganizationHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateOrganizationCommand>
{
    public async Task<Result> HandleAsync(CreateOrganizationCommand command)
    {
        // * First find the owner
        var owner = await unitOfWork.Users.GetByIdAsync(command.OwnerId);

        // ? Was the owner found?
        if (owner is null)
            return Result.Failure(new NotFoundException("The provided owner was not found, please provide a valid owner."));

        // * Create the organization
        var organization = Organization.Create(command.Name, owner);

        // ? Were there any validation errors?
        if (organization.IsFailure)
            return Result.Failure(organization.Errors.ToArray());

        // * Save the organization to the database
        await unitOfWork.Organizations.AddAsync(organization.Value);

        // ? Did the save fail?
        if (await unitOfWork.SaveChangesAsync() == 0)
            return Result.Failure(new FailedOperationException("Failed to save the organization to the database."));

        // * Set the organization's ID to the command
        command.Id = organization.Value.Id;

        // * Return the success result
        return Result.Success();
    }
}