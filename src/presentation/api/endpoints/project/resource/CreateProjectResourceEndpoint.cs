using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.resource;

[ApiExplorerSettings(GroupName = "Projects")]
public class CreateProjectResourceEndpoint(ICommandDispatcher commandDispatcher) : EndpointBase
{
    [HttpPost("project/{projectId}/resources")]
    [SwaggerOperation(Tags = new[] { "Project - Resources" })]
    public async Task<IActionResult> HandleAsync([FromRoute] string projectId, [FromBody] CreateProjectResourceRequest request)
    {
        // * Create the request
        var cmd = CreateResourceCommand.Create(request.Title, request.Url, projectId, ResourceLevel.Project);

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

    public record CreateProjectResourceRequest(string Title, string Url);

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