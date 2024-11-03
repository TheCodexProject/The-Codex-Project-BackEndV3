using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workspace.resource;

[ApiExplorerSettings(GroupName = "Workspaces")]
public class GetAllWorkspaceResourcesEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("workspace/{workspaceId}/resources")]
    [SwaggerOperation(Tags = new[] { "Workspace - Resources" })]
    public async Task<IActionResult> GetAllWorkspaceResources([FromRoute] string workspaceId)
    {
        // * Create the request
        var cmd = GetAllResourcesCommand.Create(workspaceId, ResourceLevel.Workspace);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetAllResourcesCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(TransformList(cmd)); // * Return the resources
    }

    private List<DTOs.ResourceDTO> TransformList(GetAllResourcesCommand cmd)
    {
        // * Extract the projects from the command
        var resources = cmd.Resources;

        // * Transform the projects into DTOs
        // For each project, create a DTO
        return resources.Select(TransformSingle).ToList();
    }

    private DTOs.ResourceDTO TransformSingle(Resource resource)
    {
        // * Create the DTO
        return new DTOs.ResourceDTO(resource.Id.ToString(), resource.Title, string.IsNullOrEmpty(resource.Description)? "No description..." : resource.Description,  resource.Url, resource.Type.ToString());
    }
    
}