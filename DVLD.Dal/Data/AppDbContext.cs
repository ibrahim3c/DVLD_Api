using DVLD.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Dal.Data;

public class AppDbContext: IdentityDbContext<AppUser, AppRole, string>
{
    public DbSet<Applicant> Applicants { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<ApplicationType> applicationTypes { get; set; }
    public DbSet<TestType> testTypes { get; set; }
    public DbSet<Application> applications { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>().ToTable("Users");
        builder.Entity<AppRole>().ToTable(name: "Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

}



