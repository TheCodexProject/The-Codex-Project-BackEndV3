using api.endpoints.common;
using application.appEntry.commands.user;
using application.appEntry.interfaces;
using domain.models.user;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.user;

public class GetUserEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUser([FromRoute] string id)
    {
        // * Create the request
        var cmd = GetUserCommand.Create(id);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetUserCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(new GetUserResponse(cmd.Value.User)); // * Return the user
    }

    private record GetUserResponse(User? User);
}