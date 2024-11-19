using api.endpoints.common;
using application.appEntry.commands.user;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.user;

[ApiExplorerSettings(GroupName = "Users")]
public class DeleteUserEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpDelete("users/{id}")]
    [SwaggerOperation(Tags = new[] { "User" })]
    public async Task<IActionResult> DeleteUser([FromRoute] string id)
    {
        // * Create the request
        var cmd = DeleteUserCommand.Create(id);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<DeleteUserCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(); // * Return the ID of the created user
    }
    
}