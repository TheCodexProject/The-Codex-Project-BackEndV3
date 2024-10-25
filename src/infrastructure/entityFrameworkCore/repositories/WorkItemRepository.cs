using domain.interfaces;
using domain.models.workItem;
using Microsoft.EntityFrameworkCore;

namespace entityFrameworkCore.repositories;

public class WorkItemRepository(LocalDbContext context) : IRepository<WorkItem>
{
    public async Task<IEnumerable<WorkItem>> GetAllAsync()
    {
        return await context.WorkItems.ToListAsync();
    }

    public async Task<WorkItem?> GetByIdAsync(Guid id)
    {
        return await context.WorkItems.FirstOrDefaultAsync(workItem => workItem.Id == id);
    }

    public async Task AddAsync(WorkItem toAdd)
    {
        await context.WorkItems.AddAsync(toAdd);
    }

    public void Update(WorkItem toUpdate)
    {
        context.WorkItems.Update(toUpdate);
    }

    public void Remove(WorkItem toRemove)
    {
        context.WorkItems.Remove(toRemove);
    }
    
}