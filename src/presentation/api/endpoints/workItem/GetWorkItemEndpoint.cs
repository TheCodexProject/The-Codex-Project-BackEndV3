using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workItem;

[ApiExplorerSettings(GroupName = "WorkItems")]
public class GetWorkItemEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("/workItems/{id}")]
    [SwaggerOperation(Tags = new[] { "WorkItem" })]
    public async Task<IActionResult> GetWorkItem([FromRoute] string id)
    {
        // * Create the request
        var cmd = GetWorkItemCommand.Create(id);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetWorkItemCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(Transform(cmd));
    }

    private DTOs.WorkItemDTO Transform(GetWorkItemCommand cmd)
    {
        // * Extract the workItem from the command
        var workItem = cmd.WorkItem;

        // * Create the DTO
        return new DTOs.WorkItemDTO(workItem.Id.ToString(), workItem.Project.Title.ToString(), workItem.Title, workItem.Description, workItem.Status.ToString(), workItem.Priority.ToString(), workItem.Type.ToString(), workItem.AssignedTo?.Email ?? "Unassigned");
    }
    
}