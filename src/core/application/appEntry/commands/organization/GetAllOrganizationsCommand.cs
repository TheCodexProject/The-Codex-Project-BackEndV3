using domain.models.organization;
using OperationResult;

namespace application.appEntry.commands.organization;

public class GetAllOrganizationsCommand
{
    public List<Organization> Organizations { get; set; } = [];

    private GetAllOrganizationsCommand()
    {
    }

    public static Result<GetAllOrganizationsCommand> Create()
    {
        return new GetAllOrganizationsCommand();
    }
}