using api.endpoints.common;
using api.endpoints.common.DTOs;
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
            : Ok(Transform(cmd));
    }

    private WorkItemDTO Transform(CreateWorkItemCommand cmd)
    {
        // * Extract the workItem from the command
        var workItem = cmd.WorkItem;

        // * Create the DTO
        return new WorkItemDTO(
            workItem.Id.ToString(),
            workItem.Project.Title,
            workItem.Title,
            workItem.Description,
            workItem.Status.ToString(),
            workItem.Priority.ToString(),
            workItem.Type.ToString(),
            workItem.AssignedTo?.Email ?? "Unassigned");
    }
}

public record CreateWorkItemRequest(string Title, string ProjectId);

public record CreateWorkItemResponse(string Id);