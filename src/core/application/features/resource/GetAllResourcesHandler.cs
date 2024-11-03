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

public class GetAllResourcesHandler(IUnitOfWork unitOfWork) : ICommandHandler<GetAllResourcesCommand>
{
    public async Task<Result> HandleAsync(GetAllResourcesCommand command)
    {
        // * Firstly, Identify the owner of the resources
        var owner = await GetOwnerAsync(command.Level, command.OwnerId);

        // ? Was the owner found?
        if(owner.IsFailure)
            // ! Return the error
            return Result.Failure(owner.Errors.ToArray());

        // * Initialize a list for the resources.
        var resources = owner.Value.Resources;

        // ? Were there any resources?
        var enumerable = resources.ToList();

        if (enumerable.Count == 0)
            // ! Return the error
            return Result.Failure(new NotFoundException("No resources were found in the database."));

        // * Return the resources
        command.Resources = enumerable;
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