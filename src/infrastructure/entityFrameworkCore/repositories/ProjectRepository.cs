using domain.interfaces;
using domain.models.project;
using Microsoft.EntityFrameworkCore;

namespace entityFrameworkCore.repositories;

public class ProjectRepository(LocalDbContext context) : IRepository<Project>
{
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await context.Projects.ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await context.Projects.FirstOrDefaultAsync(project => project.Id == id);
    }

    public async Task AddAsync(Project toAdd)
    {
        await context.Projects.AddAsync(toAdd);
    }

    public void Update(Project toUpdate)
    {
        context.Projects.Update(toUpdate);
    }

    public void Remove(Project toRemove)
    {
        context.Projects.Remove(toRemove);
    }
}