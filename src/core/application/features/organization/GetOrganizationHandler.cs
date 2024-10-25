using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.organization;

public class GetOrganizationHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetOrganizationCommand>
{
    public async Task<Result> HandleAsync(GetOrganizationCommand command)
    {
        // * Get the organization from the database
        var organization = await unitOfWork.Organizations.GetByIdAsync(command.Id);

        // ? Was the organization found?
        if (organization is null)
            // ! Return the errors
            return Result.Failure(new NotFoundException("The organization was not found."));

        // * Return the organization
        command.Organization = organization;
        return Result.Success();
    }
}