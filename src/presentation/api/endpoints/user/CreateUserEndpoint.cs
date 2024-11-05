using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.user;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.user;

[ApiExplorerSettings(GroupName = "Users")]
public class CreateUserEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPost("users")]
    [SwaggerOperation(Tags = new[] { "User" })]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        // * Create the request
        var cmd = CreateUserCommand.Create(request.FirstName, request.LastName, request.Email);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<CreateUserCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(Transform(cmd)); // * Return the ID of the created user
    }
    public record CreateUserRequest(string FirstName, string LastName, string Email);

    private DTOs.UserDTO Transform(CreateUserCommand cmd)
    {
        if (cmd.User is null)
        {
            return new DTOs.UserDTO("", "", "", "");
        }

        return new DTOs.UserDTO(cmd.User.Id.ToString(), cmd.User.FirstName, cmd.User.LastName, cmd.User.Email);
    }





}