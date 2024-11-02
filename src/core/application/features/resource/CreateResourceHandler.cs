using application.appEntry.commands.resource;
using application.appEntry.interfaces;
using domain.exceptions;
using domain.interfaces;
using domain.models.organization;
using domain.models.project;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.workspace;
using OperationResult;

namespace application.features.resource;

public class CreateResourceHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateResourceCommand>
{
    public async Task<Result> HandleAsync(CreateResourceCommand command)
    {
        // * Start by finding the "Owner" of the resource
        var owner = await GetOwnerAsync(command.Level, command.OwnerId);

        // ? Was the owner found?
        if(owner.IsFailure)
            // ! Return the error
            return Result.Failure(owner.Errors.ToArray());

        // * Create the resource
        var resource = Resource.Create(command.Title, command.Url, command.OwnerId, command.Level);

        // ? Was the resource created successfully?
        if(resource.IsFailure)
            // ! Return the error
            return Result.Failure(resource.Errors.ToArray());

        // * Add the resource to the owner
        var addResourceResult = owner.Value.AddResource(resource.Value);

        // ? Were there any validation errors?
        if(addResourceResult.IsFailure)
            // ! Return the error
            return Result.Failure(addResourceResult.Errors.ToArray());

        // Save the resource to the database
        await unitOfWork.Resources.AddAsync(resource);

        // ? Did the save fail?
        if (await unitOfWork.SaveChangesAsync() == 0)
            // ! Return the error
            return Result.Failure(new FailedOperationException("Failed to save the resource to the database."));

        // * Return the success result
        command.Id = resource.Value.Id;
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

    private Result UpdateOwnerAsync(IResourceOwner owner)
    {
        // Update the owner in the database
        switch (owner)
        {
            case Organization organization:
                unitOfWork.Organizations.Update(organization);
                break;

            case Workspace workspace:
                unitOfWork.Workspaces.Update(workspace);
                break;

            case Project project:
                unitOfWork.Projects.Update(project);
                break;

            default:
                return Result.Failure(new NotFoundException("The owner of the resource could not be found."));
        }

        return Result.Success();
    }

}