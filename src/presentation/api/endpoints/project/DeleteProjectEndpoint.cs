using api.endpoints.common;
using application.appEntry.commands.project;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.project;

[ApiExplorerSettings(GroupName = "Projects")]
public class DeleteProjectEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpDelete("/projects/{id}")]
    [SwaggerOperation(Tags = new[] { "Project" })]
    public async Task<IActionResult> DeleteProject([FromRoute] string id)
    {
        // * Create the request
        var cmd = DeleteProjectCommand.Create(id);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<DeleteProjectCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(); // * Return the ID of the created user
    }
}