using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.user;
using application.appEntry.interfaces;
using domain.models.user;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.user;

[ApiExplorerSettings(GroupName = "Users")]
public class UpdateUserEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPut("users/{id}")]
    [SwaggerOperation(Tags = new[] { "User" })]
    public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateUserRequest request)
    {
        // * Create the request
        var cmd = UpdateUserCommand.Create(id, request.FirstName, request.LastName, request.Email);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<UpdateUserCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(Transform(cmd)); // * Return the ID of the created user
    }

    public record UpdateUserRequest(string? FirstName, string? LastName, string? Email);

    private UserDTO Transform(UpdateUserCommand cmd)
    {
        if (cmd.User is null)
        {
            return new UserDTO("", "", "", "",[],[]);
        }

        return new UserDTO(cmd.User.Id.ToString(), cmd.User.FirstName, cmd.User.LastName, cmd.User.Email,cmd.User.OwnedOrganizations.Select(x=>x.Id.ToString()).ToList(),cmd.User.Memberships.Select(x=>x.Id.ToString()).ToList());
    }
}