using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.models.organization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.organization;

[ApiExplorerSettings(GroupName = "Organizations")]
public class GetAllOrganizationsEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("organizations")]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public async Task<IActionResult> GetAllOrganizations()
    {
        // * Create the request
        var cmd = GetAllOrganizationsCommand.Create();

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetAllOrganizationsCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(TransformList(cmd)); // * Return the organizations
    }

    private List<OrganizationDTO> TransformList(GetAllOrganizationsCommand cmd)
    {
        // * Extract the organizations from the command
        var organizations = cmd.Organizations;

        // * Transform the organizations into DTOs
        // For each organization, create a DTO
        return organizations.Select(TransformSingle).ToList();
    }

    private OrganizationDTO TransformSingle(Organization organization)
    {
        // * Extract the owner from the organization
        var owner = new UserDTO(
            organization.Owner.Id.ToString(),
            organization.Owner.FirstName,
            organization.Owner.LastName,
            organization.Owner.Email,
            organization.Owner.OwnedOrganizations.Select(x => x.Id.ToString()).ToList(),
            organization.Owner.Memberships.Select(x => x.Id.ToString()).ToList()
            );

        // * Extract the members from the organization
        var members = organization.Members.Select(member => member.Id.ToString()).ToList();

        // * Create the DTO
        return new OrganizationDTO(organization.Id.ToString(), organization.Name, owner, members);
    }
}