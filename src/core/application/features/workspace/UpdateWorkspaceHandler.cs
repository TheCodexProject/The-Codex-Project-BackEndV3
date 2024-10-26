using application.appEntry.commands.workspace;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.workspace;

public class UpdateWorkspaceHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateWorkspaceCommand>
{
    public async Task<Result> HandleAsync(UpdateWorkspaceCommand command)
    {
        // ? Does the workspace exist?
        var workspace = await unitOfWork.Workspaces.GetByIdAsync(command.Id);

        // ? If the workspace does not exist
        if (workspace == null)
            return Result.Failure(new NotFoundException("The given workspace could not be found"));

        // * Update the workspace
        if (IsChanged(workspace.Title, command.Title))
            workspace.UpdateTitle(command.Title);

        // ? Has new contacts to add?
        if (command.ContactsToAdd != null)
        {
            // Loop through the contacts to add
            foreach (var contact in command.ContactsToAdd)
            {
                // Find the contacts in the database
                var toAdd = await unitOfWork.Users.GetByIdAsync(contact);

                // ? If the contact does not exist
                if (toAdd == null)
                    return Result.Failure(new NotFoundException("The given contact could not be found"));

                // Add the contact to the workspace
                workspace.AddContact(toAdd);
            }
        }

        // ? Has contacts to remove?
        if (command.ContactsToRemove != null)
        {
            // Loop through the contacts to remove
            foreach (var contact in command.ContactsToRemove)
            {
                // Find the contacts in the database
                var toRemove = await unitOfWork.Users.GetByIdAsync(contact);

                // ? If the contact does not exist
                if (toRemove == null)
                    return Result.Failure(new NotFoundException("The given contact could not be found"));

                // Remove the contact from the workspace
                workspace.RemoveContact(toRemove);
            }
        }

        // ? Has new Projects to add?
        if (command.ProjectsToAdd != null)
        {
            // Loop through the Projects to add
            foreach (var project in command.ProjectsToAdd)
            {
                // Find the Projects in the database
                var toAdd = await unitOfWork.Projects.GetByIdAsync(project);

                // ? If the project does not exist
                if (toAdd == null)
                    return Result.Failure(new NotFoundException("The given project could not be found"));

                // Add the Projects to the workspace
                workspace.AddProject(toAdd);
            }
        }

        // ? Has Projects to remove?
        if (command.ProjectsToRemove != null)
        {
            // Loop through the Projects to remove
            foreach (var project in command.ProjectsToRemove)
            {
                // Find the Projects in the database
                var toRemove = await unitOfWork.Projects.GetByIdAsync(project);

                // ? If the project does not exist
                if (toRemove == null)
                    return Result.Failure(new NotFoundException("The given project could not be found"));

                // Remove the Projects from the workspace
                workspace.RemoveProject(toRemove);
            }
        }

        // * Save the workspace to the database
        await unitOfWork.SaveChangesAsync();

        // * Return success
        command.Workspace = workspace;
        return Result.Success();
    }

    private static bool IsChanged(string? value, string? newValue)
    {
        return value != newValue && !string.IsNullOrWhiteSpace(newValue);
    }
}