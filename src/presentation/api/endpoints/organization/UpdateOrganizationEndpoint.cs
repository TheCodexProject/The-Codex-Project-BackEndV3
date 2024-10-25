using api.endpoints.common;
using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.models.organization;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.organization;

public class UpdateOrganizationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPut("organizations/{id}")]
    public async Task<IActionResult> UpdateOrganization([FromRoute] string id, [FromBody] UpdateOrganizationRequest request)
    {
        // * Create the request
        var cmd = UpdateOrganizationCommand.Create(id, request.Name, request.MembersToAdd, request.MembersToRemove);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<UpdateOrganizationCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(Transform(cmd)); // * Return the ID of the created organization
    }

    public record UpdateOrganizationRequest(string? Name, List<string>? MembersToAdd, List<string>? MembersToRemove);

    private OrganizationDTO Transform(UpdateOrganizationCommand cmd)
    {
        if (cmd.Organization is null)
        {
            return new OrganizationDTO("","", new UserDTO("", "", ""), new List<UserDTO>());
        }

        // Handle the possibility of null Owner or Members
        var owner = cmd.Organization.Owner != null
            ? new UserDTO(cmd.Organization.Owner.Id.ToString(), $"{cmd.Organization.Owner.FirstName} {cmd.Organization.Owner.LastName}", cmd.Organization.Owner.Email)
            : new UserDTO("", "", "");

        var members = cmd.Organization.Members != null
            ? cmd.Organization.Members.Select(x => new UserDTO(x.Id.ToString(), $"{x.FirstName} {x.LastName}", x.Email)).ToList()
            : new List<UserDTO>();

        var dto = new OrganizationDTO(cmd.Organization.Id.ToString(),cmd.Organization.Name, owner, members);

        return dto;
    }

    private record OrganizationDTO(string Id,string Name, UserDTO Owner, List<UserDTO> Members);

    private record UserDTO(string Id, string Name, string Email);
}