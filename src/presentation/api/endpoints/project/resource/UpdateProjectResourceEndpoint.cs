using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.resource;

[ApiExplorerSettings(GroupName = "Projects")]
public class UpdateProjectResourceEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPut("project/{projectId}/resources/{resourceId}")]
    [SwaggerOperation(Tags = new[] { "Project - Resources" })]
    public async Task<IActionResult> UpdateProjectResource([FromRoute] string projectId, [FromRoute] string resourceId, [FromBody] UpdateProjectResourceRequest request)
    {
        // * Create a command
        var command = UpdateResourceCommand.Create(resourceId, projectId, ResourceLevel.Project, request.Title, request.Url, request.Description, request.Type);

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

    public record UpdateProjectResourceRequest(string? Title, string? Url, string? Description, string? Type);

    private DTOs.ResourceDTO Transform(UpdateResourceCommand command)
    {
        // * Extract the resource from the command
        var resource = command.Resource;

        // * Create the DTO
        return new DTOs.ResourceDTO(resource.Id.ToString(), resource.Title, string.IsNullOrEmpty(resource.Description)? "No description..." : resource.Description,  resource.Url, resource.Type.ToString());
    }

}