using api.endpoints.common;
using api.endpoints.organization.models;
using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using domain.models.workspace;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.workspace;

public class GetAllWorkspacesEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("/workspaces")]
    public async Task<IActionResult> GetAllWorkspaces()
    {
        // * Create the request
        var cmd = GetAllWorkspacesCommand.Create();

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetAllWorkspacesCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(TransformList(cmd));
    }

    private List<DTOs.WorkspaceDTO> TransformList(GetAllWorkspacesCommand cmd)
    {
        // * Extract the workspaces from the command
        var workspaces = cmd.Workspaces;

        // * Transform the workspaces into DTOs
        // For each workspace, create a DTO
        return workspaces.Select(TransformSingle).ToList();
    }

    private DTOs.WorkspaceDTO TransformSingle(Workspace workspace)
    {
        // * Extract the contacts from the workspace
        var contacts = workspace.Contacts.Select(contact => new DTOs.UserDTO(contact.Id.ToString(), $"{contact.FirstName} {contact.LastName}", contact.Email)).ToList();

        // * Create the DTO
        return new DTOs.WorkspaceDTO(workspace.Id.ToString(), workspace.Title, workspace.Owner.Name, contacts, workspace.Projects.Select(project => project.Id.ToString()).ToList());
    }
    
}