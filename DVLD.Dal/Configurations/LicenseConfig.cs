using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Dal.Configurations
{
    internal sealed class LicenseConfig : IEntityTypeConfiguration<License>
    {
        public void Configure(EntityTypeBuilder<License> builder)
        {

            builder.ToTable("Licenses");
            builder.HasKey(a => a.LicenseId);
            builder.Property(a => a.IssueDate)
                .IsRequired();

            builder.Property(l => l.IssueReason)
               .HasMaxLength(255) // Set a max length (optional)
               .IsUnicode(true)   // Supports Unicode characters
               .IsRequired(false); // Nullable

            builder.Property(l => l.Notes)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(a => a.PaidFees)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

            // app with apptype
            builder.HasOne(a => a.LicenseClass)
                .WithMany()
                .HasForeignKey(a => a.LicenseClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // app with applicant
            builder.HasOne(a => a.Driver)
                    .WithMany(d=>d.Licenses)
                    .HasForeignKey(a => a.DriverId)
                    .OnDelete(DeleteBehavior.Restrict);

     

            builder.HasOne(a => a.Application)
                    .WithOne()
                    .HasForeignKey<License>(a => a.AppId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
