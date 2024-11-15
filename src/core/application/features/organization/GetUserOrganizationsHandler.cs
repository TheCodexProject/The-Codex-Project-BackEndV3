using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.interfaces;
using OperationResult;

namespace application.features.organization;

public class GetUserOrganizationsHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetUserOrganizationsCommand>
{
    public async Task<Result> HandleAsync(GetUserOrganizationsCommand command)
    {
        // * Get all the organizations
        var organizations = await unitOfWork.Organizations.GetAllAsync();

        // ? Were there any organizations?
        var enumerable = organizations.ToList();

        // * Filter the organizations by user ID in owners and members
        var userOrganizations = enumerable.Where(o => o.Owner.Id == command.UserId
                                                      || o.Members.Any(m => m.Id == command.UserId)).ToList();

        // * Set the organizations
        command.Organizations = userOrganizations;

        // * Return the result
        return Result.Success();
    }
}