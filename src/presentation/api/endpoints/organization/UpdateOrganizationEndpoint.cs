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

    private DTOs.OrganizationDTO Transform(UpdateOrganizationCommand cmd)
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

        var dto = new DTOs.OrganizationDTO(cmd.Organization.Id.ToString(),cmd.Organization.Name, owner, members);

        return dto;
    }
}