using api.endpoints.common;
using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.workItem;

public class UpdateWorkItemEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPut("/workItems/{id}")]
    public async Task<IActionResult> UpdateWorkItem([FromRoute] string id, [FromBody] UpdateWorkItemRequest request)
    {
        // * Create the request
        var cmd = UpdateWorkItemCommand.Create(id, request.Title, request.Description, request.Status, request.Priority, request.Type, request.AssignedTo, request.SubItemsToAdd, request.SubItemsToRemove);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<UpdateWorkItemCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(Transform(cmd));
    }

    private UpdatedWorkItemDTO Transform(UpdateWorkItemCommand cmd)
    {
        // * Extract the workItem from the command
        var workItem = cmd.WorkItem;

        // * Create the DTO
        return new UpdatedWorkItemDTO(workItem.Id.ToString(), workItem.Project.Title.ToString(), workItem.Title, workItem.Description, workItem.Status.ToString(), workItem.Priority.ToString(), workItem.Type.ToString(), workItem.AssignedTo?.Email ?? "No assignee", workItem.Subitems?.Select(subItem => subItem.Id.ToString()).ToList() ?? new List<string>()
        );
    }

    private record UpdatedWorkItemDTO(string Id, string Project, string Title, string Description, string Status, string Priority, string Type, string AssignedTo, List<string> SubItems);
}

public record UpdateWorkItemRequest(string? Title, string? Description, string? Status, string? Priority, string? Type, string? AssignedTo, List<string>? SubItemsToAdd, List<string>? SubItemsToRemove);