using api.endpoints.common;
using api.endpoints.organization.models;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.organization.resource;

[ApiExplorerSettings(GroupName = "Organizations")]
public class UpdateOrganizationResourceEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPut("organization/{organizationId}/resource/{resourceId}")]
    [SwaggerOperation(Tags = new[] { "Organization - Resources" })]
    public async Task<IActionResult> UpdateOrganizationResource(string organizationId, string resourceId, [FromBody] UpdateResourceRequest request)
    {
        // * Create a command
        var command = UpdateResourceCommand.Create(resourceId, organizationId, ResourceLevel.Organization, request.Title, request.Url, request.Description, request.Type);

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

    public record UpdateResourceRequest(string? Title, string? Url, string? Description, string? Type);

    private DTOs.ResourceDTO Transform(UpdateResourceCommand command)
    {
        // * Extract the resource from the command
        var resource = command.Resource;

        // * Create the DTO
        return new DTOs.ResourceDTO(resource.Id.ToString(), resource.Title, string.IsNullOrEmpty(resource.Description)? "No description..." : resource.Description,  resource.Url, resource.Type.ToString());
    }

}