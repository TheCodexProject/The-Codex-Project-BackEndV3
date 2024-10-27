using api.endpoints.common;
using api.endpoints.organization.models;
using application.appEntry.commands.project;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.project;

public class UpdateProjectEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPut("/projects/{id}")]
    public async Task<IActionResult> UpdateProject([FromRoute] string id, [FromBody] UpdateProjectRequest request)
    {
        // * Create the request
        var cmd = UpdateProjectCommand.Create(id, request.Title, request.Description, request.Status, request.Priority, request.StartDate, request.EndDate);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<UpdateProjectCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(Transform(cmd));
    }

    private DTOs.ProjectDTO Transform(UpdateProjectCommand cmd)
    {
        // * Extract the project from the command
        var project = cmd.Project;

        // * Create the DTO
        return new DTOs.ProjectDTO(project.Id.ToString(), project.Title, project.Description, project.Status.ToString(), project.Priority.ToString(), [project.Start.ToString("yyyy-MM-dd HH:mm:ss"), project.End.ToString("yyyy-MM-dd HH:mm:ss")
        ], project.Workspace.Title);
    }
    
}

public record UpdateProjectRequest(string? Title, string? Description, string? Status, string? Priority, string? StartDate, string? EndDate);