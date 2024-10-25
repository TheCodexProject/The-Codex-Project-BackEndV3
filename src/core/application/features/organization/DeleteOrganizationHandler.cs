using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.organization;

public class DeleteOrganizationHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteOrganizationCommand>
{
    public async Task<Result> HandleAsync(DeleteOrganizationCommand command)
    {
        // ? Does the organization exist?
        var organization = await unitOfWork.Organizations.GetByIdAsync(command.Id);

        // ? If the organization does not exist
        if (organization == null)
            return Result.Failure(new NotFoundException("The given organization could not be found"));

        // * Delete the organization
        unitOfWork.Organizations.Remove(organization);

        // ? Was the organization deleted?
        if (await unitOfWork.SaveChangesAsync() == 0)
            return Result.Failure(new FailedOperationException("Organization could not be deleted"));

        // * Return success
        return Result.Success();
    }
}