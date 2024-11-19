using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.models.organization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.organization;

[ApiExplorerSettings(GroupName = "Organizations")]
public class GetOrganizationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("organizations/{id}")]
    [SwaggerOperation(Tags = new[] { "Organization" })]
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
        // * Extract the organization
        var org = cmd.Organization;

        // * Transform the Owner
        var owner = new UserDTO(
            org.Owner.Id.ToString(),
            org.Owner.FirstName,
            org.Owner.LastName,
            org.Owner.Email,
            org.Owner.OwnedOrganizations.Select(x => x.Id.ToString()).ToList(),
            org.Owner.Memberships.Select(x => x.Id.ToString()).ToList()
        );

        // * Transform the Members
        var members = org.Members.Select(x => x.Id.ToString()).ToList();

        // * Make the DTO
        return new OrganizationDTO(
            org.Id.ToString(),
            org.Name,
            owner,
            members
        );
    }
}