using api.endpoints.common;
using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.models.projectActivity.value;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.milestone;

[ApiExplorerSettings(GroupName = "Projects")]
public class DeleteMilestoneEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpDelete("project/{projectId}/milestones/{milestoneId}")]
    [SwaggerOperation(Tags = new [] { "Project - Milestones" })]
    public async Task<IActionResult> HandleAsync([FromRoute] string projectId, [FromRoute] string milestoneId)
    {
        // * Create the command
        var command = DeleteProjectActivityCommand.Create(milestoneId, projectId, ProjectActivityType.Milestone);

        // ? Were there any validation errors?
        if (command.IsFailure)
            return BadRequest(command.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<DeleteProjectActivityCommand>(command.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(); // * Return the success
    }

}