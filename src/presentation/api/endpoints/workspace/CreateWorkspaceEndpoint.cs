using api.endpoints.common;
using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.workspace;

public class CreateWorkspaceEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPost("/workspaces")]
    public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceRequest request)
    {
        // * Create the request
        var cmd = CreateWorkspaceCommand.Create(request.OrganizationId, request.Title);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<CreateWorkspaceCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(new CreateWorkspaceResponse(cmd.Value.Id.ToString()));
    }
}

public record CreateWorkspaceRequest(string Title, string OrganizationId);
public record CreateWorkspaceResponse(string Id);