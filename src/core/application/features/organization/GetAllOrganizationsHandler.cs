using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.organization;

public class GetAllOrganizationsHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetAllOrganizationsCommand>
{
    public async Task<Result> HandleAsync(GetAllOrganizationsCommand command)
    {
        // * Get all the organizations
        var organizations =  await  unitOfWork.Organizations.GetAllAsync();

        // ? Were there any organizations?
        var enumerable = organizations.ToList();

        // * Return the organizations
        command.Organizations = enumerable;
        return Result.Success();
    }
}