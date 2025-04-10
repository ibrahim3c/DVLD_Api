using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DVLD.Dal.Configurations
{
    internal sealed class ApplicationConfig : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("Applications");
            builder.HasKey(a => a.AppID);
            builder.Property(a=>a.AppDate)
                .IsRequired();
            builder.Property(a => a.AppStatus)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(a => a.AppFee)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

            // app with apptype
            builder.HasOne(a => a.AppType)
                .WithMany(at=>at.Applications)
                .HasForeignKey(a=>a.AppTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            // app with applicant
            builder.HasOne(a => a.Applicant)
                    .WithMany(a=>a.Applications)
                    .HasForeignKey(a => a.ApplicantId);

            // app with applicant
            builder.HasOne(a => a.LicenseClass)
                    .WithMany(a => a.Applications)
                    .HasForeignKey(a => a.LicenseClassId);


          

        }
    }
}
