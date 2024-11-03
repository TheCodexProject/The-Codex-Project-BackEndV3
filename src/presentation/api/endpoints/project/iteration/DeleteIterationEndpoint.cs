using api.endpoints.common;
using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.models.projectActivity.value;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.iteration;

[ApiExplorerSettings(GroupName = "Projects")]
public class DeleteIterationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpDelete("project/{projectId}/iterations/{iterationId}")]
    [SwaggerOperation(Tags = new [] { "Project - Iterations" })]
    public async Task<IActionResult> HandleAsync([FromRoute] string projectId, [FromRoute] string iterationId)
    {
        // * Create the command
        var command = DeleteProjectActivityCommand.Create(iterationId, projectId, ProjectActivityType.Iteration);

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