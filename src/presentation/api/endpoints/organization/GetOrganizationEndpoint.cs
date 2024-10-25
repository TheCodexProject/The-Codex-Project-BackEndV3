using api.endpoints.common;
using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.models.organization;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.organization;

public class GetOrganizationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("organizations/{id}")]
    public async Task<IActionResult> GetOrganization([FromRoute] string id)
    {
        // * Create the request
        var cmd = GetOrganizationCommand.Create(id);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetOrganizationCommand>(cmd.Value);

        // * Transform the result into a response
        var dto = Transform(cmd);


        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(dto); // * Return the organization
    }

    private OrganizationDTO Transform(GetOrganizationCommand cmd)
    {
        if (cmd.Organization is null)
        {
            return new OrganizationDTO("", new UserDTO("", "", ""), new List<UserDTO>());
        }

        // Handle the possibility of null Owner or Members
        var owner = cmd.Organization.Owner != null
            ? new UserDTO(cmd.Organization.Owner.Id.ToString(), $"{cmd.Organization.Owner.FirstName} {cmd.Organization.Owner.LastName}", cmd.Organization.Owner.Email)
            : new UserDTO("", "", "");

        var members = cmd.Organization.Members != null
            ? cmd.Organization.Members.Select(x => new UserDTO(x.Id.ToString(), $"{x.FirstName} {x.LastName}", x.Email)).ToList()
            : new List<UserDTO>();

        var dto = new OrganizationDTO(cmd.Organization.Name, owner, members);

        return dto;
    }

    private record OrganizationDTO(string Name, UserDTO Owner, List<UserDTO> Members);

    private record UserDTO(string Id, string Name, string Email);
}