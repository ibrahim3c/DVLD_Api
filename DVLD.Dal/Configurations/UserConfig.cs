using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Dal.Configurations;

internal class UserConfig : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        // Now add this in AppUser configuration
        builder.HasOne(u => u.Applicant)
            .WithOne(a => a.User)
            .HasForeignKey<Applicant>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade); // When deleting User, delete Applicant
    }
}
