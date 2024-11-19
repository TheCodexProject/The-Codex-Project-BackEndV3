using System.Globalization;
using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.project;
using application.appEntry.interfaces;
using domain.models.project;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project;

[ApiExplorerSettings(GroupName = "Projects")]
public class GetAllProjectsEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("projects")]
    [SwaggerOperation(Tags = new[] { "Project" })]
    public async Task<IActionResult> GetAllProjects([FromQuery] string? workspaceId)
    {
        // * Create the request
        if (string.IsNullOrWhiteSpace(workspaceId))
        {
            var cmd = GetAllProjectsCommand.Create();

            // * Dispatch the command
            var result = await dispatcher.DispatchAsync<GetAllProjectsCommand>(cmd.Value);

            // ? Did the execution fail?
            return result.IsFailure
                ? BadRequest(result.Errors)
                : Ok(TransformList(cmd));
        }
        else
        {
            var cmd = GetWorkspaceProjectsCommand.Create(workspaceId);

            // * Dispatch the command
            var result = await dispatcher.DispatchAsync<GetWorkspaceProjectsCommand>(cmd.Value);

            // ? Did the execution fail?
            return result.IsFailure
                ? BadRequest(result.Errors)
                : Ok(TransformList(cmd));
        }

    }

    private List<ProjectDTO> TransformList(GetAllProjectsCommand cmd)
    {
        // * Extract the projects from the command
        var projects = cmd.Projects;

        // * Transform the projects into DTOs
        // For each project, create a DTO
        return projects.Select(TransformSingle).ToList();
    }

    private List<ProjectDTO> TransformList(GetWorkspaceProjectsCommand cmd)
    {
        // * Extract the projects from the command
        var projects = cmd.Projects;

        // * Transform the projects into DTOs
        // For each project, create a DTO
        return projects.Select(TransformSingle).ToList();
    }

    private ProjectDTO TransformSingle(Project project)
    {
        // * Extract the project from the command

        // * Create the DTO
        return new ProjectDTO(project.Id.ToString(), project.Title, project.Workspace.Title, project.Description, project.Status.ToString(), project.Priority.ToString(), [project.Start.ToString("yyyy-MM-dd HH:mm:ss"), project.End.ToString("yyyy-MM-dd HH:mm:ss")
        ]);
    }
    
}