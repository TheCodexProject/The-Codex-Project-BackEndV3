using api.endpoints.common;
using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workItem;

[ApiExplorerSettings(GroupName = "WorkItems")]
public class CreateWorkItemEndpoint(ICommandDispatcher commandDispatcher) : EndpointBase
{
    [HttpPost("/workItems")]
    [SwaggerOperation(Tags = new[] { "WorkItem" })]
    public async Task<IActionResult> HandleAsync([FromBody] CreateWorkItemRequest request)
    {
        // * Create the command
        var cmd = CreateWorkItemCommand.Create(request.ProjectId, request.Title);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await commandDispatcher.DispatchAsync<CreateWorkItemCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(new CreateWorkItemResponse(cmd.Value.Id.ToString()));
    }
}

public record CreateWorkItemRequest(string Title, string ProjectId);

public record CreateWorkItemResponse(string Id);