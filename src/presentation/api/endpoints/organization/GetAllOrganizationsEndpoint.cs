using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.organization;
using application.appEntry.interfaces;
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

        // * Transform the result into a response
        var dtos = Transform(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors) // ! Return the errors
            : Ok(new GetAllOrganizationsResponse(dtos)); // * Return the organizations
    }

    private List<DTOs.OrganizationDTO> Transform(GetAllOrganizationsCommand cmd)
    {
        if (cmd == null || cmd.Organizations == null || !cmd.Organizations.Any())
        {
            return new List<DTOs.OrganizationDTO>();
        }

        return cmd.Organizations.Select(organization =>
        {
            if (organization == null)
            {
                return new DTOs.OrganizationDTO("","", new UserDTO("", "", "", "", [],[]), new List<UserDTO>());
            }

            var owner = organization.Owner != null
                ? new UserDTO(
                    organization.Owner.Id.ToString(),
                    organization.Owner.FirstName,
                    organization.Owner.LastName,
                    organization.Owner.Email,
                    organization.Owner.OwnedOrganizations != null
                        ? organization.Owner.OwnedOrganizations.Select(x => x.ToString()).ToList()
                        : [],
                    organization.Owner.Memberships != null
                        ? organization.Owner.Memberships.Select(x => x.ToString()).ToList()
                        : []
                    )
                : new UserDTO("", "", "", "", [], []);

            var members = organization.Members != null
                ? organization.Members.Select(x => new UserDTO(
                    x.Id.ToString(),
                    x.FirstName,
                    x.LastName,
                    x.Email,
                    [],
                    [])
                ).ToList()
                : [];

            return new DTOs.OrganizationDTO(organization.Id.ToString(),organization.Name, owner, members);
        }).ToList();
    }

    private record GetAllOrganizationsResponse(IEnumerable<DTOs.OrganizationDTO> Organizations);


}