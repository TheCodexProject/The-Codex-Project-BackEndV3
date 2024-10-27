using api.endpoints.common;
using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.workItem;

public class DeleteWorkItemEndpoint(ICommandDispatcher commandDispatcher) : EndpointBase
{
    [HttpDelete("/workItems/{id}")]
    public async Task<IActionResult> HandleAsync([FromRoute] string id)
    {
        // * Create the command
        var cmd = DeleteWorkItemCommand.Create(id);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await commandDispatcher.DispatchAsync<DeleteWorkItemCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok();
    }
    
}