using domain.interfaces;
using domain.models.user;
using Microsoft.EntityFrameworkCore;

namespace entityFrameworkCore.repositories;

public class UserRepository(LocalDbContext context) : IRepository<User>
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task AddAsync(User toAdd)
    {
        await context.Users.AddAsync(toAdd);
    }

    public void Update(User toUpdate)
    {
        context.Users.Update(toUpdate);
    }

    public void Remove(User toRemove)
    {
        context.Users.Remove(toRemove);
    }
    
}