using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.organization;
using domain.models.project;
using domain.models.resource.values;
using domain.models.workspace;
using OperationResult;

namespace application.features.resource;

public class UpdateResourceHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateResourceCommand>
{
    public async Task<Result> HandleAsync(UpdateResourceCommand command)
    {
        // * Firstly, Identify the owner of the resource
        var owner = await GetOwnerAsync(command.Level, command.OwnerId);

        // ? Was the owner found?
        if(owner.IsFailure)
            // ! Return the error
            return Result.Failure(owner.Errors.ToArray());

        // * Get the resource from the owner
        var resource = owner.Value.Resources.FirstOrDefault(r => r.Id == command.Resource.Id);

        // ? Was the resource found?
        if(resource is null)
            // ! Return the error
            return Result.Failure(new NotFoundException("The resource could not be found."));

        // * Update the resource
        if (IsChanged(resource.Title, command.Resource.Title))
            resource.UpdateTitle(command.Title);

        if (IsChanged(resource.Description, command.Resource.Description))
            resource.UpdateDescription(command.Description);

        if (IsChanged(resource.Url, command.Resource.Url))
            resource.UpdateUrl(command.Url);

        if (command.Type.HasValue && (command.Type != resource.Type || command.Type != ResourceType.None))
            resource.UpdateType(command.Type.Value);

        // * Save the changes
        await unitOfWork.SaveChangesAsync();

        // * Return the result
        command.Resource = resource;
        return Result.Success();
    }

    private async Task<Result<IResourceOwner>> GetOwnerAsync(ResourceLevel level, Guid ownerId)
    {
        IResourceOwner? owner;

        switch(level)
        {
            case ResourceLevel.Organization:
                // * Get the organization from the database
                owner = await unitOfWork.Organizations.GetByIdAsync(ownerId);

                // ? Was the organization found?
                if(owner is null)
                    // ! Return the error
                    return Result<IResourceOwner>.Failure(new NotFoundException("The organization could not be found."));
                break;

            case ResourceLevel.Workspace:
                // * Get the workspace from the database
                owner = await unitOfWork.Workspaces.GetByIdAsync(ownerId);

                // ? Was the workspace found?
                if(owner is null)
                    // ! Return the error
                    return Result<IResourceOwner>.Failure(new NotFoundException("The workspace could not be found."));
                break;

            case ResourceLevel.Project:
                // * Get the project from the database
                owner = await unitOfWork.Projects.GetByIdAsync(ownerId);

                // ? Was the project found?
                if(owner is null)
                    // ! Return the error
                    return Result<IResourceOwner>.Failure(new NotFoundException("The project could not be found."));
                break;

            case ResourceLevel.None:
            default:
                return Result<IResourceOwner>.Failure(new NotFoundException("The owner of the resource could not be found."));
        }

        return Result<IResourceOwner>.Success(owner);
    }

    private static bool IsChanged(string? value, string? newValue)
    {
        return value != newValue && !string.IsNullOrWhiteSpace(newValue);
    }
}