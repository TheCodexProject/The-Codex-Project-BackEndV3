﻿using domain.interfaces;
using domain.models.organization;
using domain.models.project;
using domain.models.user;
using domain.models.workItem;
using domain.models.workspace;

namespace entityFrameworkCore;

public class UnitOfWork(
    LocalDbContext context,
    IRepository<Workspace> workspaces,
    IRepository<WorkItem> workItems,
    IRepository<User> users,
    IRepository<Project> projects,
    IRepository<Organization> organizations)
    : IUnitOfWork, IDisposable
{
    #region Repostiories

    public IRepository<User> Users { get; } = users;
    public IRepository<Organization> Organizations { get; } = organizations;
    public IRepository<Workspace> Workspaces { get; } = workspaces;
    public IRepository<Project> Projects { get; } = projects;
    public IRepository<WorkItem> WorkItems { get; } = workItems;

    #endregion

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}