using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using domain.models.workspace;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workspace;

[ApiExplorerSettings(GroupName = "Workspaces")]
public class GetAllWorkspacesEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("workspaces")]
    [SwaggerOperation(Tags = new[] { "Workspace" })]
    public async Task<IActionResult> GetAllWorkspaces([FromQuery] string? organizationId)
    {
        // * Create the request
        if(string.IsNullOrEmpty(organizationId))
        {
            var cmd = GetAllWorkspacesCommand.Create();

            // * Dispatch the command
            var result = await dispatcher.DispatchAsync<GetAllWorkspacesCommand>(cmd.Value);

            // ? Did the execution fail?
            return result.IsFailure
                ? BadRequest(result.Errors)
                : Ok(TransformList(cmd));
        }
        else
        {
            var cmd = GetOrganizationWorkspacesCommand.Create(organizationId);

            // * Dispatch the command
            var result = await dispatcher.DispatchAsync<GetOrganizationWorkspacesCommand>(cmd.Value);

            // ? Did the execution fail?
            return result.IsFailure
                ? BadRequest(result.Errors)
                : Ok(TransformList(cmd));
        }
    }

    private List<WorkspaceDTO> TransformList(GetAllWorkspacesCommand cmd)
    {
        // * Extract the workspaces from the command
        var workspaces = cmd.Workspaces;

        // * Transform the workspaces into DTOs
        // For each workspace, create a DTO
        return workspaces.Select(TransformSingle).ToList();
    }

    private List<WorkspaceDTO> TransformList(GetOrganizationWorkspacesCommand cmd)
    {
        // * Extract the workspaces from the command
        var workspaces = cmd.Workspaces;

        // * Transform the workspaces into DTOs
        // For each workspace, create a DTO
        return workspaces.Select(TransformSingle).ToList();
    }

    private WorkspaceDTO TransformSingle(Workspace workspace)
    {
        // * Extract the contacts from the workspace
        var contacts = workspace.Contacts.Select(contact => new UserDTO(contact.Id.ToString(), contact.FirstName, contact.LastName, contact.Email,[],[])).ToList();

        // * Create the DTO
        return new WorkspaceDTO(workspace.Id.ToString(), workspace.Title, workspace.Owner.Name, contacts, workspace.Projects.Select(project => project.Id.ToString()).ToList());
    }
    
}