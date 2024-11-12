using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workspace.resource;

[ApiExplorerSettings(GroupName = "Workspaces")]
public class CreateWorkspaceResourceEndpoint(ICommandDispatcher commandDispatcher) : EndpointBase
{
    [HttpPost("workspace/{workspaceId}/resources")]
    [SwaggerOperation(Tags = new[] { "Workspace - Resources" })]
    public async Task<IActionResult> HandleAsync([FromRoute] string workspaceId, [FromBody] CreateWorkspaceResourceRequest request)
    {
        // * Create the request
        var cmd = CreateResourceCommand.Create(request.Title, request.Url, workspaceId, ResourceLevel.Workspace);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await commandDispatcher.DispatchAsync<CreateResourceCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(Transform(cmd)); // * Return the ID of the created resource
    }

    public record CreateWorkspaceResourceRequest(string Title, string Url);

    private static ResourceDTO Transform(CreateResourceCommand cmd)
    {
        // * Extract the resource from the command
        var resource = cmd.Resource;

        // ? Is the resource null?
        return resource is null ?
            new ResourceDTO("", "", "", "", "") // ! Return a blank DTO
            : new ResourceDTO(resource.Id.ToString(), resource.Title, string.IsNullOrEmpty(resource.Description)? "No description..." : resource.Description,  resource.Url, resource.Type.ToString());
    }

}