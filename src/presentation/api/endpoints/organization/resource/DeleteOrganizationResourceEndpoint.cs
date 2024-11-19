using api.endpoints.common;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.organization.resource;

[ApiExplorerSettings(GroupName = "Organizations")]
public class DeleteOrganizationResourceEndpoint(ICommandDispatcher commandDispatcher) : EndpointBase
{
    [HttpDelete("organization/{organizationId}/resources/{resourceId}")]
    [SwaggerOperation(Tags = new[] { "Organization - Resources" })]
    public async Task<IActionResult> DeleteOrganizationResource([FromRoute] string organizationId, [FromRoute] string resourceId)
    {
        // * Create the command
        var command = DeleteResourceCommand.Create(resourceId, organizationId, ResourceLevel.Organization);

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