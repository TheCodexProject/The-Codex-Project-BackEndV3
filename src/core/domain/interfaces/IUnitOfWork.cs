using domain.models.organization;
using domain.models.project;
using domain.models.user;
using domain.models.workItem;
using domain.models.workspace;

namespace domain.interfaces;

/// <summary>
/// Unit of Work interface
/// </summary>
public interface IUnitOfWork
{
    // ! Must have access to all repositories
    IRepository<User> Users { get; }
    IRepository<Organization> Organizations { get; }
    IRepository<Workspace> Workspaces { get; }
    IRepository<Project> Projects { get; }
    IRepository<WorkItem> WorkItems { get; }

    /// <summary>
    /// Save changes to the database
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();
}