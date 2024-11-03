using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.models.projectActivity.value;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.iteration;

[ApiExplorerSettings(GroupName = "Projects")]
public class GetIterationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("project/{projectId}/iterations/{iterationId}")]
    [SwaggerOperation(Tags = new[] { "Project - Iterations" })]
    public async Task<IActionResult> HandleAsync([FromRoute] string projectId, [FromRoute] string iterationId)
    {
        // * Create the command
        var command = GetProjectActivityCommand.Create(iterationId, projectId , ProjectActivityType.Iteration);

        // ? Were there any validation errors?
        if (command.IsFailure)
            return BadRequest(command.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetProjectActivityCommand>(command.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(Transform(command)); // * Return the success
    }

    private DTOs.ActivityDTO Transform(GetProjectActivityCommand command)
    {
        // * Extract the project activity from the command
        var projectActivity = command.ProjectActivity;

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