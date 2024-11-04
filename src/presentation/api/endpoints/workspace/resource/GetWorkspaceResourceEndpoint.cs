using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workspace.resource;

[ApiExplorerSettings(GroupName = "Workspaces")]
public class GetWorkspaceResourceEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("workspace/{workspaceId}/resources/{resourceId}")]
    [SwaggerOperation(Tags = new[] { "Workspace - Resources" })]
    public async Task<IActionResult> GetWorkspaceResource([FromRoute] string workspaceId, [FromRoute] string resourceId)
    {
        // * Create the command
        var command = GetResourceCommand.Create(resourceId, workspaceId, ResourceLevel.Workspace);

        // ? Were there any validation errors?
        if (command.IsFailure)
            return BadRequest(command.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetResourceCommand>(command.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(Transform(command)); // * Return the resource
    }

    private DTOs.ResourceDTO Transform(GetResourceCommand command)
    {
        // * Extract the resource from the command
        var resource = command.Resource;

        // * Create the DTO
        return new DTOs.ResourceDTO(resource.Id.ToString(), resource.Title, string.IsNullOrEmpty(resource.Description)? "No description..." : resource.Description,  resource.Url, resource.Type.ToString());
    }
}