using domain.models.organization;
using domain.models.project;
using domain.models.user;
using domain.models.workItem;
using domain.models.workspace;
using entityFrameworkCore.configs;
using Microsoft.EntityFrameworkCore;

namespace entityFrameworkCore;

public class LocalDbContext : DbContext
{
    // # TABLES #
    public DbSet<User> Users { get; init; }
    public DbSet<Organization> Organizations { get; init; }
    public DbSet<Workspace> Workspaces { get; init; }
    public DbSet<Project> Projects { get; init; }
    public DbSet<WorkItem> WorkItems { get; init; }

    // # CONFIGURATION #
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=localdb.db");
        }
    }


    // # MODEL CONFIGURATION #
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // * Apply the Entity specific configurations.
        modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspaceConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}