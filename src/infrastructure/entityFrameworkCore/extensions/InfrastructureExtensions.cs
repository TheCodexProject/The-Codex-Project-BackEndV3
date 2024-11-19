using domain.interfaces;
using domain.models.organization;
using domain.models.project;
using domain.models.projectActivity;
using domain.models.resource;
using domain.models.user;
using domain.models.workItem;
using domain.models.workspace;
using entityFrameworkCore.repositories;
using Microsoft.Extensions.DependencyInjection;

namespace entityFrameworkCore.extensions;

public static class InfrastructureExtensions
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Workspace>, WorkspaceRepository>();
        services.AddScoped<IRepository<WorkItem>, WorkItemRepository>();
        services.AddScoped<IRepository<User>, UserRepository>();
        services.AddScoped<IRepository<Project>, ProjectRepository>();
        services.AddScoped<IRepository<Organization>, OrganizationRepository>();
        services.AddScoped<IRepository<Resource>, ResourceRepository>();
        services.AddScoped<IRepository<ProjectActivity>, ProjectActivityRepository>();
    }

    public static void RegisterUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}