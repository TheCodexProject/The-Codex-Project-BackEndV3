using domain.models.organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace entityFrameworkCore.configs;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        // * Many-to-Many : Organization (Many) to User (Many) * (Members/Memberships)
        builder
            .HasMany(o => o.Members)
            .WithMany(u => u.Memberships);

        // * Many-to-One : Organization (Many) to User (1) * (Owner/OwnedOrganizations)
        builder
            .HasOne(o => o.Owner)
            .WithMany(u => u.OwnedOrganizations)
            .HasForeignKey("_ownerId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}