using api.endpoints.common;
using application.appEntry.commands.project;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project;

[ApiExplorerSettings(GroupName = "Projects")]
public class CreateProjectEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPost("/projects")]
    [SwaggerOperation(Tags = new[] { "Project" })]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        // * Create the request
        var cmd = CreateProjectCommand.Create(request.WorkspaceId, request.Title);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<CreateProjectCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(new CreateProjectResponse(cmd.Value.Id.ToString()));
    }
}

public record CreateProjectRequest(string Title, string WorkspaceId);
public record CreateProjectResponse(string Id);