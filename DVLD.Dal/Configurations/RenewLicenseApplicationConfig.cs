using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Dal.Configurations
{
    public class RenewLicenseApplicationConfig : IEntityTypeConfiguration<RenewLicenseApplication>
    {
        public void Configure(EntityTypeBuilder<RenewLicenseApplication> builder)
        {
            builder.HasOne(l => l.ExpiredLicense)
                .WithOne()
                .HasForeignKey<RenewLicenseApplication>(l=>l.ExpiredLicenseId);
        }
    }
}
