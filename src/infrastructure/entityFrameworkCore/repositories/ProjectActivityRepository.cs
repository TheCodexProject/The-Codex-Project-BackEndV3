using domain.interfaces;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using Microsoft.EntityFrameworkCore;

namespace entityFrameworkCore.repositories;

/// <summary>
/// The repository for project iterations
/// </summary>
public class ProjectActivityRepository(LocalDbContext context) : IRepository<ProjectActivity>
{
    public async Task<IEnumerable<ProjectActivity>> GetAllAsync()
    {
        return await context.ProjectActivities
            .ToListAsync();
    }

    public async Task<ProjectActivity?> GetByIdAsync(Guid id)
    {
        return await context.ProjectActivities
            .FirstOrDefaultAsync(projectActivity => projectActivity.Id == id);
    }

    public async Task AddAsync(ProjectActivity toAdd)
    {
        await context.ProjectActivities.AddAsync(toAdd);
    }

    public void Update(ProjectActivity toUpdate)
    {
        context.ProjectActivities.Update(toUpdate);
    }

    public void Remove(ProjectActivity toRemove)
    {
        context.ProjectActivities.Remove(toRemove);
    }
}