using api.endpoints.common;
using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.organization;

public class CreateOrganizationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPost("organizations")]
    public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequest request)
    {
        // * Create the request
        var cmd = CreateOrganizationCommand.Create(request.Name, request.OwnerId);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<CreateOrganizationCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(new CreateOrganizationResponse(cmd.Value.Id.ToString()));
    }
}

public record CreateOrganizationRequest(string Name, string OwnerId);

public record CreateOrganizationResponse(string Id);
