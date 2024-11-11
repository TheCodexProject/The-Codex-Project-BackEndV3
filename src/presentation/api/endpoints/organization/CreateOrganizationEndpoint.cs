using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.organization;

[ApiExplorerSettings(GroupName = "Organizations")]
public class CreateOrganizationEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPost("organizations")]
    [SwaggerOperation(Tags = new[] { "Organization" })]
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
            : Ok(Transform(cmd));
    }

    private OrganizationDTO Transform(CreateOrganizationCommand cmd)
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

public record CreateOrganizationRequest(string Name, string OwnerId);
