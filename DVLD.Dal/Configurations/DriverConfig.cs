using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Dal.Configurations
{
    internal sealed class DriverConfig : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable("Drivers");
            builder.HasKey(d=>d.DriverId);
            builder.HasOne(d => d.Applicant)
                .WithOne()
                .HasForeignKey<Driver>(d => d.applicantId);

        }
    }
}
