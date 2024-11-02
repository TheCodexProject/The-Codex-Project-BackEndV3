using application.appEntry.commands.organization;
using application.appEntry.commands.project;
using application.appEntry.commands.resource;
using application.appEntry.commands.user;
using application.appEntry.commands.workItem;
using application.appEntry.commands.workspace;
using application.appEntry.dispatcher;
using application.appEntry.interfaces;
using application.features.organization;
using application.features.project;
using application.features.resource;
using application.features.user;
using application.features.workItem;
using application.features.workspace;
using Microsoft.Extensions.DependencyInjection;

namespace application.extensions;

public static class ApplicationExtensions
{
    public static void RegisterCommandHandlers(this IServiceCollection services)
    {
        // * ------------ *
        // * User Handlers *
        // * ------------ *
        services.AddScoped<ICommandHandler<CreateUserCommand>,CreateUserHandler>();
        services.AddScoped<ICommandHandler<GetUserCommand>, GetUserHandler>();
        services.AddScoped<ICommandHandler<GetAllUsersCommand>, GetAllUsersHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>,UpdateUserHandler>();
        services.AddScoped<ICommandHandler<DeleteUserCommand>, DeleteUserHandler>();

        // * --------------------- *
        // * Organization Handlers *
        // * --------------------- *
        services.AddScoped<ICommandHandler<CreateOrganizationCommand>,CreateOrganizationHandler>();
        services.AddScoped<ICommandHandler<GetOrganizationCommand>, GetOrganizationHandler>();
        services.AddScoped<ICommandHandler<GetAllOrganizationsCommand>, GetAllOrganizationsHandler>();
        services.AddScoped<ICommandHandler<UpdateOrganizationCommand>,UpdateOrganizationHandler>();
        services.AddScoped<ICommandHandler<DeleteOrganizationCommand>, DeleteOrganizationHandler>();

        // * ------------------ *
        // * Workspace Handlers *
        // * ------------------ *
        services.AddScoped<ICommandHandler<CreateWorkspaceCommand>,CreateWorkspaceHandler>();
        services.AddScoped<ICommandHandler<GetWorkspaceCommand>, GetWorkspaceHandler>();
        services.AddScoped<ICommandHandler<GetAllWorkspacesCommand>, GetAllWorkspacesHandler>();
        services.AddScoped<ICommandHandler<UpdateWorkspaceCommand>,UpdateWorkspaceHandler>();
        services.AddScoped<ICommandHandler<DeleteWorkspaceCommand>, DeleteWorkspaceHandler>();

        // * ---------------- *
        // * Project Handlers *
        // * ---------------- *
        services.AddScoped<ICommandHandler<CreateProjectCommand>,CreateProjectHandler>();
        services.AddScoped<ICommandHandler<GetProjectCommand>, GetProjectHandler>();
        services.AddScoped<ICommandHandler<GetAllProjectsCommand>, GetAllProjectsHandler>();
        services.AddScoped<ICommandHandler<UpdateProjectCommand>,UpdateProjectHandler>();
        services.AddScoped<ICommandHandler<DeleteProjectCommand>, DeleteProjectHandler>();

        // * ----------------- *
        // * WorkItem Handlers *
        // * ----------------- *
        services.AddScoped<ICommandHandler<CreateWorkItemCommand>,CreateWorkItemHandler>();
        services.AddScoped<ICommandHandler<GetWorkItemCommand>, GetWorkItemHandler>();
        services.AddScoped<ICommandHandler<GetAllWorkItemsCommand>, GetAllWorkItemsHandler>();
        services.AddScoped<ICommandHandler<UpdateWorkItemCommand>,UpdateWorkItemHandler>();
        services.AddScoped<ICommandHandler<DeleteWorkItemCommand>, DeleteWorkItemHandler>();

        // * ----------------- *
        // * Resource Handlers *
        // * ----------------- *
        services.AddScoped<ICommandHandler<CreateResourceCommand>,CreateResourceHandler>();
        services.AddScoped<ICommandHandler<GetResourceCommand>, GetResourceHandler>();
        services.AddScoped<ICommandHandler<GetAllResourcesCommand>, GetAllResourcesHandler>();
        services.AddScoped<ICommandHandler<UpdateResourceCommand>,UpdateResourceHandler>();
        services.AddScoped<ICommandHandler<DeleteResourceCommand>, DeleteResourceHandler>();
    }


    public static void RegisterCommandDispatcher(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
    }
}