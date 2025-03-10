using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DVLD.Core.Helpers;

namespace DVLD.Dal.Configurations;

public class ApplicantConfig : IEntityTypeConfiguration<Applicant>
{
    public void Configure(EntityTypeBuilder<Applicant> builder)
    {
        builder.ToTable("applicants");

        builder.HasKey(a => a.ApplicantId);
        builder.Property(a => a.NationalNo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Fname)
           .IsRequired()
           .HasMaxLength(50);

        builder.Property(a => a.Sname)
           .IsRequired()
           .HasMaxLength(50);

        builder.Property(a => a.Tname)
           .IsRequired()
           .HasMaxLength(50);

        builder.Property(a => a.Lname)
           .IsRequired()
           .HasMaxLength(50);

        builder.Property(a => a.BirthDate)
           .IsRequired();

        builder.Property(a => a.Address)
          .IsRequired()
          .HasMaxLength(255);

        builder.Property(a => a.ImagePath)
            .IsRequired(false);

        builder.Property(a => a.Gender)
           .IsRequired()
           .HasConversion(
               a => (int)a,  // Convert `Gender` enum to `int` before saving to DB
               a => (Gender)a // Convert `int` back to `Gender` enum when reading from DB
           );
        //one country => many applicants
        builder.HasOne(a => a.Country)
            .WithMany(e=>e.Applicants)
            .HasForeignKey(a=>a.CountryId);

        builder.HasOne(a => a.User)
             .WithOne(a=>a.Applicant)
            .HasForeignKey<Applicant>(a=>a.UserId)
            .OnDelete(DeleteBehavior.NoAction);

}

}
