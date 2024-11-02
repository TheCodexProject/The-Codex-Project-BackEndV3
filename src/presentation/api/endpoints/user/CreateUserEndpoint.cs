using api.endpoints.common;
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
            : Ok(new CreateUserResponse(cmd.Value.Id.ToString())); // * Return the ID of the created user
    }
    public record CreateUserRequest(string FirstName, string LastName, string Email);
    public record CreateUserResponse(string Id);





}