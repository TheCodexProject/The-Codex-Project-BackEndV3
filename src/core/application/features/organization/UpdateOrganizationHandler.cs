using application.appEntry.commands.organization;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using OperationResult;

namespace application.features.organization;

public class UpdateOrganizationHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateOrganizationCommand>
{
    public async Task<Result> HandleAsync(UpdateOrganizationCommand command)
    {
        // ? Does the organization exist?
        var organization = await unitOfWork.Organizations.GetByIdAsync(command.Id);

        // ? If the organization does not exist
        if (organization == null)
            return Result.Failure(new NotFoundException("The given organization could not be found"));


        // * Update the organization
        if (IsChanged(organization.Name, command.Name))
            organization.UpdateName(command.Name);

        // ? Has new members to add?
        if (command.MembersToAdd != null)
        {
            // Loop through the members to add
            foreach (var member in command.MembersToAdd)
            {
                // Find the members in the database
                var toAdd = await unitOfWork.Users.GetByIdAsync(member);

                // ? If the member does not exist
                if (toAdd == null)
                    return Result.Failure(new NotFoundException("The given user could not be found"));

                // Add the member to the organization
                organization.AddMember(toAdd);
            }
        }

        // ? Has members to remove?
        if (command.MembersToRemove != null)
        {
            // Loop through the members to remove
            foreach (var member in command.MembersToRemove)
            {
                // Find the members in the database
                var toRemove = await unitOfWork.Users.GetByIdAsync(member);

                // ? If the member does not exist
                if (toRemove == null)
                    return Result.Failure(new NotFoundException("The given user could not be found"));

                // Remove the member from the organization
                organization.RemoveMember(toRemove);
            }
        }

        // * Save the organization to the database
        await unitOfWork.SaveChangesAsync();

        // * Return success
        command.Organization = organization;
        return Result.Success();
    }

    private static bool IsChanged(string? value, string? newValue)
    {
        return value != newValue && !string.IsNullOrWhiteSpace(newValue);
    }
}