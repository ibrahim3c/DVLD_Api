using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Dal.Configurations
{
    internal sealed class ApplicationTypeConfig : IEntityTypeConfiguration<ApplicationType>
    {
        public void Configure(EntityTypeBuilder<ApplicationType> builder)
        {
            builder.ToTable("ApplicationTypes");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .HasMaxLength(255);

            builder.Property(a => a.TypeFee)
                .IsRequired()
                .HasColumnType("decimal(18,2)");


            builder.HasData(
              new ApplicationType
              {
                  Id = 1,
                  Title = "New Local Driving License Service",
                  Description = "Apply for a new local driving license.",
                  TypeFee = 15.00000M
              },
              new ApplicationType
              {
                  Id = 2,
                  Title = "Renew Driving License Service",
                  Description = "Renew an existing driving license.",
                  TypeFee = 5.00000M
              },
              new ApplicationType
              {
                  Id = 3,
                  Title = "Replacement for a Lost Driving License",
                  Description = "Get a replacement for a lost license.",
                  TypeFee = 10.00000M
              },
              new ApplicationType
              {
                  Id = 4,
                  Title = "Replacement for a Damaged Driving License",
                  Description = "Replace a damaged driving license.",
                  TypeFee = 5.00000M
              },
              new ApplicationType
              {
                  Id = 5,
                  Title = "Release Detained Driving License",
                  Description = "Release a detained driving license.",
                  TypeFee = 15.00000M
              },
              new ApplicationType
              {
                  Id = 6,
                  Title = "New International Driving License",
                  Description = "Apply for an international driving license.",
                  TypeFee = 51.00000M
              },
              new ApplicationType
              {
                  Id = 7,
                  Title = "Retake Test",
                  Description = "Retake a driving test after failure.",
                  TypeFee = 5.00000M
              }
          );
        }
    }

}
