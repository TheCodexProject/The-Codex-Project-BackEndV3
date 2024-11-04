using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.iteration;

[ApiExplorerSettings(GroupName = "Projects")]
public class UpdateIterationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPut("project/{projectId}/iterations/{iterationId}")]
    [SwaggerOperation(Tags = new[] { "Project - Iterations" })]
    public async Task<IActionResult> HandleAsync([FromRoute] string projectId, [FromRoute] string iterationId, [FromBody] UpdateIterationRequest request)
    {
        // * Create the command
        var command = UpdateProjectActivityCommand.Create(iterationId, projectId , ProjectActivityType.Iteration, request.Title, request.Description, request.ItemsToAdd, request.ItemsToRemove);

        // ? Were there any validation errors?
        if (command.IsFailure)
            return BadRequest(command.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<UpdateProjectActivityCommand>(command.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(Transform(command)); // * Return the success
    }

    public record UpdateIterationRequest(string? Title, string? Description, List<string>? ItemsToAdd, List<string>? ItemsToRemove);

    private DTOs.ActivityDTO Transform(UpdateProjectActivityCommand command)
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