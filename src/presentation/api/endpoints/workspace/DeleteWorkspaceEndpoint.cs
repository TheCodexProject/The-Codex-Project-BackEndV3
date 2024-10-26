using api.endpoints.common;
using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.workspace;

public class DeleteWorkspaceEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpDelete("/workspaces/{id}")]
    public async Task<IActionResult> DeleteWorkspace([FromRoute] string id)
    {
        // * Create the request
        var cmd = DeleteWorkspaceCommand.Create(id);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<DeleteWorkspaceCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(); // * Return the ID of the created user
    }
    
}