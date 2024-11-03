using domain.interfaces;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using Microsoft.EntityFrameworkCore;

namespace entityFrameworkCore.repositories;

public class MilestoneRepository(LocalDbContext context) : IRepository<ProjectActivity>
{
    public async Task<IEnumerable<ProjectActivity>> GetAllAsync()
    {
        return await context.ProjectActivities
            .Where(projectActivity => projectActivity.Type == ProjectActivityType.Milestone)
            .ToListAsync();
    }

    public async Task<ProjectActivity?> GetByIdAsync(Guid id)
    {
        return await context.ProjectActivities
            .Where(projectActivity => projectActivity.Type == ProjectActivityType.Milestone)
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