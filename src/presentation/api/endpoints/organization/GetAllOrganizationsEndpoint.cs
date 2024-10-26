using api.endpoints.common;
using api.endpoints.organization.models;
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
                return new DTOs.OrganizationDTO("","", new DTOs.UserDTO("", "", ""), new List<DTOs.UserDTO>());
            }

            var owner = organization.Owner != null
                ? new DTOs.UserDTO(organization.Owner.Id.ToString(), $"{organization.Owner.FirstName} {organization.Owner.LastName}", organization.Owner.Email)
                : new DTOs.UserDTO("", "", "");

            var members = organization.Members != null
                ? organization.Members.Select(x => new DTOs.UserDTO(x.Id.ToString(), $"{x.FirstName} {x.LastName}", x.Email)).ToList()
                : new List<DTOs.UserDTO>();

            return new DTOs.OrganizationDTO(organization.Id.ToString(),organization.Name, owner, members);
        }).ToList();
    }



    private record GetAllOrganizationsResponse(IEnumerable<DTOs.OrganizationDTO> Organizations);


}