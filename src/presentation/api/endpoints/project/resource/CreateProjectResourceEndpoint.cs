using api.endpoints.common;
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
        var cmd = CreateResourceCommand.Create(request.title, request.url, projectId, ResourceLevel.Project);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await commandDispatcher.DispatchAsync<CreateResourceCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(new CreateResourceResponse(cmd.Value.Id.ToString())); // * Return the ID of the created resource
    }

    public record CreateProjectResourceRequest(string title, string url);

    private record CreateResourceResponse(string Id);

}