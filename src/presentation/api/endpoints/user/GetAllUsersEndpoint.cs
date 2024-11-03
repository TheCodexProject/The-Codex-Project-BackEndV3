using api.endpoints.common;
using application.appEntry.commands.user;
using application.appEntry.interfaces;
using domain.models.user;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.user;

[ApiExplorerSettings(GroupName = "Users")]
public class GetAllUsersEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("users")]
    [SwaggerOperation(Tags = new[] { "User" })]
    public async Task<IActionResult> GetAllUsers()
    {
        // * Create the request
        var cmd = GetAllUsersCommand.Create();

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetAllUsersCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(new GetAllUsersResponse(cmd.Value.Users)); // * Return the users
    }

    private record GetAllUsersResponse(IEnumerable<User> Users);
    
}