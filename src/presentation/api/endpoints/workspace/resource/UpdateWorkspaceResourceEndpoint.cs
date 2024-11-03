using api.endpoints.common;
using api.endpoints.organization.models;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workspace.resource;

[ApiExplorerSettings(GroupName = "Workspaces")]
public class UpdateWorkspaceResourceEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPut("workspace/{workspaceId}/resources/{resourceId}")]
    [SwaggerOperation(Tags = new[] { "Workspace - Resources" })]
    public async Task<IActionResult> UpdateWorkspaceResource([FromRoute] string workspaceId, [FromRoute] string resourceId, [FromBody] UpdateWorkspaceResourceRequest request)
    {
        // * Create a command
        var command = UpdateResourceCommand.Create(resourceId, workspaceId, ResourceLevel.Workspace, request.Title, request.Url, request.Description, request.Type);

        // ? Were there any validation errors?
        if (command.IsFailure)
            return BadRequest(command.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<UpdateResourceCommand>(command);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(Transform(command)); // * Return the resource
    }

    public record UpdateWorkspaceResourceRequest(string? Title, string? Url, string? Description, string? Type);

    private DTOs.ResourceDTO Transform(UpdateResourceCommand command)
    {
        // * Extract the resource from the command
        var resource = command.Resource;

        // * Create the DTO
        return new DTOs.ResourceDTO(resource.Id.ToString(), resource.Title, string.IsNullOrEmpty(resource.Description)? "No description..." : resource.Description,  resource.Url, resource.Type.ToString());
    }
    
}