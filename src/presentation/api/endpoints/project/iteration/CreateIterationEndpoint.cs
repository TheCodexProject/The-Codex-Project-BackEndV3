using api.endpoints.common;
using application.appEntry.commands.projectActivity;
using application.appEntry.interfaces;
using domain.models.projectActivity.value;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project.iteration;

[ApiExplorerSettings(GroupName = "Projects")]
public class CreateIterationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPost("project/{projectId}/iterations")]
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
            : Ok(new CreateIterationResponse(command.Value.Id.ToString())); // * Return the ID of the created iteration
    }

    public record CreateIterationRequest(string Title);

    private record CreateIterationResponse(string Id);
}