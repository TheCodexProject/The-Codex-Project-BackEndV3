using api.endpoints.common;
using api.endpoints.common.DTOs;
using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.endpoints.workspace;

[ApiExplorerSettings(GroupName = "Workspaces")]
public class CreateWorkspaceEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPost("workspaces")]
    [SwaggerOperation(Tags = new[] { "Workspace" })]
    public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceRequest request)
    {
        // * Create the request
        var cmd = CreateWorkspaceCommand.Create(request.OrganizationId, request.Title);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<CreateWorkspaceCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(Transform(cmd));
    }

    private WorkspaceDTO Transform(CreateWorkspaceCommand cmd)
    {
        // * Extract the workspace from the command
        var workspace = cmd.Workspace;

        // * Extract the contacts from the workspace
        var contacts = workspace.Contacts.Select(contact => new UserDTO(contact.Id.ToString(), contact.FirstName, contact.LastName, contact.Email, [], [])).ToList();

        // * Create the DTO
        return new WorkspaceDTO(workspace.Id.ToString(), workspace.Title, workspace.Owner.Name, contacts, workspace.Projects.Select(project => project.Id.ToString()).ToList());
    }
}

public record CreateWorkspaceRequest(string Title, string OrganizationId);
public record CreateWorkspaceResponse(string Id);