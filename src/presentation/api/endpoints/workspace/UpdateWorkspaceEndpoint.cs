using api.endpoints.common;
using api.endpoints.organization.models;
using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using domain.models.project;
using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.workspace;

public class UpdateWorkspaceEndpoint(ICommandDispatcher dispatcher) : EndpointBase
{
    [HttpPut("/workspaces/{id}")]
    public async Task<IActionResult> UpdateWorkspace([FromRoute] string id, [FromBody] UpdateWorkspaceRequest request)
    {
        // * Create the request
        var cmd = UpdateWorkspaceCommand.Create(id, request.Title, request.ContactsToAdd, request.ContactsToRemove, request.ProjectsToAdd, request.ProjectsToRemove);

        // ? Were there any validation errors?
        if (cmd.IsFailure)
            return BadRequest(cmd.Errors);

        // * Dispatch the command
        var result = await dispatcher.DispatchAsync<UpdateWorkspaceCommand>(cmd.Value);

        // ? Did the execution fail?
        return result.IsFailure
            ? BadRequest(result.Errors)
            : Ok(Transform(cmd));
    }

    private DTOs.WorkspaceDTO Transform(UpdateWorkspaceCommand cmd)
    {
        // * Extract the workspace from the command
        var workspace = cmd.Workspace;

        // * Extract the contacts from the workspace
        var contacts = workspace.Contacts.Select(contact => new DTOs.UserDTO(contact.Id.ToString(), $"{contact.FirstName} {contact.LastName}", contact.Email)).ToList();

        // * Create the DTO
        return new DTOs.WorkspaceDTO(workspace.Id.ToString(), workspace.Title, workspace.Owner.Name, contacts, workspace.Projects.Select(project => project.Id.ToString()).ToList());
    }
}

public record UpdateWorkspaceRequest(string? Title, List<string>? ContactsToAdd, List<string>? ContactsToRemove, List<string>? ProjectsToAdd, List<string>? ProjectsToRemove);





