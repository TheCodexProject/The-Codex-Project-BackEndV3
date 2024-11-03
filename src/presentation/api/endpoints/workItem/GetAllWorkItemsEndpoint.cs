using api.endpoints.common;
using api.endpoints.organization.models;
using application.appEntry.commands.workItem;
using application.appEntry.interfaces;
using domain.models.workItem;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workItem;

[ApiExplorerSettings(GroupName = "WorkItems")]
public class GetAllWorkItemsEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("/workItems")]
    [SwaggerOperation(Tags = new[] { "WorkItem" })]
    public async Task<IActionResult> GetAllWorkItems()
    {
        // * Create the request
        var cmd = GetAllWorkItemsCommand.Create();

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetAllWorkItemsCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(TransformList(cmd));
    }

    private List<DTOs.WorkItemDTO> TransformList(GetAllWorkItemsCommand cmd)
    {
        // * Extract the workItems from the command
        var workItems = cmd.WorkItems;

        // * Transform the workItems into DTOs
        // For each workItem, create a DTO
        return workItems.Select(TransformSingle).ToList();
    }

    private DTOs.WorkItemDTO TransformSingle(WorkItem workItem)
    {
        // * Create the DTO
        return new DTOs.WorkItemDTO(workItem.Id.ToString(), workItem.Project.Title.ToString(), workItem.Title, workItem.Description, workItem.Status.ToString(), workItem.Priority.ToString(), workItem.Type.ToString(), workItem.AssignedTo?.Email ?? "Unassigned");
    }
    
}