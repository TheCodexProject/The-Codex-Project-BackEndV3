using domain.interfaces;
using domain.models.organization;
using Microsoft.EntityFrameworkCore;

namespace entityFrameworkCore.repositories;

public class OrganizationRepository(LocalDbContext context) : IRepository<Organization>
{
    public async Task<IEnumerable<Organization>> GetAllAsync()
    {
        return await context.Organizations
            .Include(organization => organization.Owner)
            .Include(organization => organization.Members)
            .ToListAsync();
    }

    public async Task<Organization?> GetByIdAsync(Guid id)
    {
        return await context.Organizations
            .Include(organization => organization.Owner)
            .Include(organization => organization.Members)
            .Include(organization => organization.Resources)
            .FirstOrDefaultAsync(organization => organization.Id == id);
    }

    public async Task AddAsync(Organization toAdd)
    {
        await context.Organizations.AddAsync(toAdd);
    }

    public void Update(Organization toUpdate)
    {
        context.Organizations.Update(toUpdate);
    }

    public void Remove(Organization toRemove)
    {
        context.Organizations.Remove(toRemove);
    }
}