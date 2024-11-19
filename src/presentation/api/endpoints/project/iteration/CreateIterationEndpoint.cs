using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.models.projectActivity.value;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.iteration;

[ApiExplorerSettings(GroupName = "Projects")]
public class CreateIterationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPost("projects/{projectId}/iterations")]
    [SwaggerOperation(Tags = new[] { "Project - Iterations" })]
    public async Task<IActionResult> HandleAsync([FromRoute] string projectId, [FromBody] CreateIterationRequest request)
    {
        // * Create the command
        var command = CreateProjectActivityCommand.Create(projectId, request.Title, ProjectActivityType.Iteration);

        // ? Were there any validation errors?
        if (command.IsFailure)
            return BadRequest(command.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<CreateProjectActivityCommand>(command.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(Transform(command)); // * Return the ID of the created iteration
    }

    public record CreateIterationRequest(string Title);

    private static ActivityDTO Transform(CreateProjectActivityCommand command)
    {
        // * Extract the project activity from the command
        var projectActivity = command.ProjectActivity;

        // ? Is the project activity null?
        if (projectActivity is null)
            return new ActivityDTO("", "", "", "", []);

        // * Create the DTO
        return new ActivityDTO(
            projectActivity.Id.ToString(),
            projectActivity.Project.Id.ToString(),
            projectActivity.Title,
            projectActivity.Description ?? "No description...",
            projectActivity.WorkItems.Select(item => item.Id.ToString()).ToList()
        );
    }
}