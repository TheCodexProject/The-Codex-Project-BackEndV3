using api.endpoints.common;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workspace.resource;

[ApiExplorerSettings(GroupName = "Workspaces")]
public class DeleteWorkspaceResourceEndpoint(ICommandDispatcher commandDispatcher) : EndpointBase
{
    [HttpDelete("workspace/{workspaceId}/resources/{resourceId}")]
    [SwaggerOperation(Tags = new[] { "Workspace - Resources" })]
    public async Task<IActionResult> DeleteWorkspaceResource([FromRoute] string workspaceId, [FromRoute] string resourceId)
    {
        // * Create the command
        var command = DeleteResourceCommand.Create(resourceId, workspaceId, ResourceLevel.Workspace);

        // ? Was the command created successfully?
        if (command.IsFailure)
            // ! Return the error
            return BadRequest(command.Errors);

        // * Dispatch the command
        var result = await commandDispatcher.DispatchAsync<DeleteResourceCommand>(command.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(); // * Return the success
    }

}