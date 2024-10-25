using api.endpoints.common;
using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.models.organization;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.organization;

public class GetAllOrganizationsEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("organizations")]
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

    private List<OrganizationDTO> Transform(GetAllOrganizationsCommand cmd)
    {
        if (cmd == null || cmd.Organizations == null || !cmd.Organizations.Any())
        {
            return new List<OrganizationDTO>();
        }

        return cmd.Organizations.Select(organization =>
        {
            if (organization == null)
            {
                return new OrganizationDTO("", new UserDTO("", "", ""), new List<UserDTO>());
            }

            var owner = organization.Owner != null
                ? new UserDTO(organization.Owner.Id.ToString(), $"{organization.Owner.FirstName} {organization.Owner.LastName}", organization.Owner.Email)
                : new UserDTO("", "", "");

            var members = organization.Members != null
                ? organization.Members.Select(x => new UserDTO(x.Id.ToString(), $"{x.FirstName} {x.LastName}", x.Email)).ToList()
                : new List<UserDTO>();

            return new OrganizationDTO(organization.Name, owner, members);
        }).ToList();
    }

    private record OrganizationDTO(string Name, UserDTO Owner, List<UserDTO> Members);

    private record UserDTO(string Id, string Name, string Email);

    private record GetAllOrganizationsResponse(IEnumerable<OrganizationDTO> Organizations);


}