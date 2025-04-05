using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Dal.Configurations
{
    public class DetainedLicenseConfig : IEntityTypeConfiguration<DetainedLicense>
    {
        public void Configure(EntityTypeBuilder<DetainedLicense> builder)
        {
            builder.ToTable("DetainedLicenses");

            // Primary Key
            builder.HasKey(dl => dl.DetainedLicenseId);

            // Properties
            builder.Property(dl => dl.FineFees)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); // FineFees as decimal with a precision of 18 and scale of 2

            builder.Property(dl => dl.DetainedDate)
                .IsRequired();

            builder.Property(dl => dl.ReleasedDate)
                .IsRequired(false); // ReleasedDate is nullable since it can be null initially

            builder.Property(dl => dl.Reason)
                .HasMaxLength(500)
                .IsRequired(true);

            builder.Property(dl => dl.IsReleased)
                .IsRequired();

            builder.Property(dl => dl.Notes)
                .HasMaxLength(500)
                .IsRequired(false); // Nullable field for notes

            // Relationships

            // License to DetainedLicense - One-to-One relationship
            builder.HasOne(dl => dl.License)
                .WithMany(l => l.DetainedLicenses)
                .HasForeignKey(dl => dl.LicenseId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Application for release (One-to-One relationship)
            builder.HasOne(dl => dl.ReleaseApplication)
                .WithOne() 
                .HasForeignKey<DetainedLicense>(dl => dl.ReleaseApplicationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }
    }
  }
