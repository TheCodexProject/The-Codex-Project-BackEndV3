using domain.interfaces;
using domain.models.resource;
using Microsoft.EntityFrameworkCore;

namespace entityFrameworkCore.repositories;

public class ResourceRepository(LocalDbContext context) : IRepository<Resource>
{
    public async Task<IEnumerable<Resource>> GetAllAsync()
    {
        return await context.Resources.ToListAsync();
    }

    public async Task<Resource?> GetByIdAsync(Guid id)
    {
        return await context.Resources.FirstOrDefaultAsync(resource => resource.Id == id);
    }

    public async Task AddAsync(Resource toAdd)
    {
        await context.Resources.AddAsync(toAdd);
    }

    public void Update(Resource toUpdate)
    {
        context.Resources.Update(toUpdate);
    }

    public void Remove(Resource toRemove)
    {
        context.Resources.Remove(toRemove);
    }
}