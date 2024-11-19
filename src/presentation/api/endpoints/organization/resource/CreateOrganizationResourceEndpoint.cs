using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.organization.resource;

[ApiExplorerSettings(GroupName = "Organizations")]
public class CreateOrganizationResourceEndpoint(ICommandDispatcher commandDispatcher) : EndpointBase
{
    [HttpPost("organization/{organizationId}/resources")]
    [SwaggerOperation(Tags = new[] { "Organization - Resources" })]
    public async Task<IActionResult> HandleAsync([FromRoute] string organizationId, [FromBody] CreateOrganizationResourceRequest request)
    {
        // * Create the request
        var cmd = CreateResourceCommand.Create(request.Title, request.Url, organizationId, ResourceLevel.Organization);

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

    private static ResourceDTO Transform(CreateResourceCommand cmd)
    {
        // * Extract the resource from the command
        var resource = cmd.Resource;

        // ? Is the resource null?
        return resource is null ?
            new ResourceDTO("", "", "", "", "") // ! Return a blank DTO
            : new ResourceDTO(resource.Id.ToString(), resource.Title, string.IsNullOrEmpty(resource.Description)? "No description..." : resource.Description,  resource.Url, resource.Type.ToString());
    }

    public record CreateOrganizationResourceRequest(string Title, string Url);
}