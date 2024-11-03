using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.project;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project;

[ApiExplorerSettings(GroupName = "Projects")]
public class GetProjectEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("/projects/{id}")]
    [SwaggerOperation(Tags = new[] { "Project" })]
    public async Task<IActionResult> GetProject([FromRoute] string id)
    {
        // * Create the request
        var cmd = GetProjectCommand.Create(id);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetProjectCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(Transform(cmd));
    }

    private DTOs.ProjectDTO Transform(GetProjectCommand cmd)
    {
        // * Extract the project from the command
        var project = cmd.Project;

        // * Create the DTO
        return new DTOs.ProjectDTO(project.Id.ToString(), project.Title, project.Description, project.Status.ToString(), project.Priority.ToString(), [project.Start.ToString("yyyy-MM-dd HH:mm:ss"), project.End.ToString("yyyy-MM-dd HH:mm:ss")
        ], project.Workspace.Title);
    }
    
}