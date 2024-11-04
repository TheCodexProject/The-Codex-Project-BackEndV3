using api.endpoints.common;
using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.models.projectActivity.value;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.milestone;

[ApiExplorerSettings(GroupName = "Projects")]
public class CreateMilestoneEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPost("project/{projectId}/milestones")]
    [SwaggerOperation(Tags = new[] { "Project - Milestones" })]
    public async Task<IActionResult> HandleAsync([FromRoute] string projectId, [FromBody] CreateMilestoneRequest request)
    {
        // * Create the command
        var command = CreateProjectActivityCommand.Create(projectId, request.Title, ProjectActivityType.Milestone);

        // ? Were there any validation errors?
        if (command.IsFailure)
            return BadRequest(command.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<CreateProjectActivityCommand>(command.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(new CreateMilestoneResponse(command.Value.Id.ToString())); // * Return the ID of the created iteration
    }

    public record CreateMilestoneRequest(string Title);

    private record CreateMilestoneResponse(string Id);
}