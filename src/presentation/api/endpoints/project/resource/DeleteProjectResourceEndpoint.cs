using api.endpoints.common;
using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.models.resource.values;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.resource;

[ApiExplorerSettings(GroupName = "Projects")]
public class DeleteProjectResourceEndpoint(ICommandDispatcher commandDispatcher) : EndpointBase
{
    [HttpDelete("project/{projectId}/resources/{resourceId}")]
    [SwaggerOperation(Tags = new[] { "Project - Resources" })]
    public async Task<IActionResult> DeleteProjectResource([FromRoute] string projectId, [FromRoute] string resourceId)
    {
        // * Create the command
        var command = DeleteResourceCommand.Create(resourceId, projectId, ResourceLevel.Project);

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