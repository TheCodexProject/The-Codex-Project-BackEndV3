using api.endpoints.common;
using api.endpoints.organization.models;
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

    private DTOs.OrganizationDTO Transform(GetOrganizationCommand cmd)
    {
        if (cmd.Organization is null)
        {
            return new DTOs.OrganizationDTO("","", new DTOs.UserDTO("", "", ""), new List<DTOs.UserDTO>());
        }

        // Handle the possibility of null Owner or Members
        var owner = cmd.Organization.Owner != null
            ? new DTOs.UserDTO(cmd.Organization.Owner.Id.ToString(), $"{cmd.Organization.Owner.FirstName} {cmd.Organization.Owner.LastName}", cmd.Organization.Owner.Email)
            : new DTOs.UserDTO("", "", "");

        var members = cmd.Organization.Members != null
            ? cmd.Organization.Members.Select(x => new DTOs.UserDTO(x.Id.ToString(), $"{x.FirstName} {x.LastName}", x.Email)).ToList()
            : new List<DTOs.UserDTO>();

        var dto = new DTOs.OrganizationDTO(cmd.Organization.Id.ToString(), cmd.Organization.Name, owner, members);

        return dto;
    }
}