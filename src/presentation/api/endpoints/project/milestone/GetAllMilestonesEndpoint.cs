using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.milestone;

[ApiExplorerSettings(GroupName = "Projects")]
public class GetAllMilestonesEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("project/{projectId}/milestones")]
    [SwaggerOperation(Tags = new[] { "Project - Milestones" })]
    public async Task<IActionResult> HandleAsync([FromRoute] string projectId)
    {
        // * Create the command
        var command = GetAllProjectActivitiesCommand.Create(projectId, ProjectActivityType.Milestone);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetAllProjectActivitiesCommand>(command);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(TransformList(command)); // * Return the success
    }

    private List<DTOs.ActivityDTO> TransformList(GetAllProjectActivitiesCommand command)
    {
        // * Extract the project activities from the command
        var projectActivities = command.ProjectActivities;

        // * Transform the project activities into DTOs
        // For each project activity, transform it into a DTO
        return projectActivities.Select(Transform).ToList();
    }

    private DTOs.ActivityDTO Transform(ProjectActivity projectActivity)
    {
        // * Create the DTO
        return new DTOs.ActivityDTO(
            projectActivity.Id.ToString(),
            projectActivity.Project.Id.ToString(),
            projectActivity.Title,
            projectActivity.Description ?? "No description...",
            projectActivity.WorkItems.Select(item => item.Id.ToString()).ToList()
        );
    }
}