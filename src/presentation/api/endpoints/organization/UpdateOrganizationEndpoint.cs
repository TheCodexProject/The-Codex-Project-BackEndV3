using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.models.organization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.organization;

[ApiExplorerSettings(GroupName = "Organizations")]
public class UpdateOrganizationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPut("organizations/{id}")]
    [SwaggerOperation(Tags = new[] { "Organization" })]
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