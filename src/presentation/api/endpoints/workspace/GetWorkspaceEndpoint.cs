using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workspace;

[ApiExplorerSettings(GroupName = "Workspaces")]
public class GetWorkspaceEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpGet("/workspaces/{id}")]
    [SwaggerOperation(Tags = new[] { "Workspace" })]
    public async Task<IActionResult> GetWorkspace([FromRoute] string id)
    {
        // * Create the request
        var cmd = GetWorkspaceCommand.Create(id);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<GetWorkspaceCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(Transform(cmd));
    }

    private DTOs.WorkspaceDTO Transform(GetWorkspaceCommand cmd)
    {
        // * Extract the workspace from the command
        var workspace = cmd.Workspace;

        // * Extract the contacts from the workspace
        var contacts = workspace.Contacts.Select(contact => new DTOs.UserDTO(contact.Id.ToString(), $"{contact.FirstName} {contact.LastName}", contact.Email)).ToList();

        // * Create the DTO
        return new DTOs.WorkspaceDTO(workspace.Id.ToString(), workspace.Title, workspace.Owner.Name, contacts, workspace.Projects.Select(project => project.Id.ToString()).ToList());
    }
}