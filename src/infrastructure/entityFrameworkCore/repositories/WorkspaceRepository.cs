using domain.interfaces;
using domain.models.workspace;
using Microsoft.EntityFrameworkCore;

namespace entityFrameworkCore.repositories;

public class WorkspaceRepository(LocalDbContext context) : IRepository<Workspace>
{
    public async Task<IEnumerable<Workspace>> GetAllAsync()
    {
        return await context.Workspaces
            .Include(workspace => workspace.Contacts)
            .Include(workspace => workspace.Projects)
            .Include(workspace => workspace.Owner)
            .ToListAsync();
    }

    public async Task<Workspace?> GetByIdAsync(Guid id)
    {
        return await context.Workspaces
            .Include(workspace => workspace.Contacts)
            .Include(workspace => workspace.Projects)
            .Include(workspace => workspace.Owner)
            .FirstOrDefaultAsync(workspace => workspace.Id == id);
    }

    public async Task AddAsync(Workspace toAdd)
    {
        await context.Workspaces.AddAsync(toAdd);
    }

    public void Update(Workspace toUpdate)
    {
        context.Workspaces.Update(toUpdate);
    }

    public void Remove(Workspace toRemove)
    {
        context.Workspaces.Remove(toRemove);
    }
}