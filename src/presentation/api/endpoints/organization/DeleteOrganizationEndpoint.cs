using api.endpoints.common;
using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.organization;

public class DeleteOrganizationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpDelete("organizations/{id}")]
    public async Task<IActionResult> DeleteOrganization([FromRoute] string id)
    {
        // * Create the request
        var cmd = DeleteOrganizationCommand.Create(id);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<DeleteOrganizationCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(); // * Return the ID of the created user
    }
    
}