using domain.models.user;
using OperationResult;

namespace application.appEntry.commands.user;

public class GetAllUsersCommand
{
    public List<User> Users { get; set; } = [];

    private GetAllUsersCommand() { }

    public static Result<GetAllUsersCommand> Create()
    {
        // ! No validation needed here.
        return new GetAllUsersCommand();
    }

}