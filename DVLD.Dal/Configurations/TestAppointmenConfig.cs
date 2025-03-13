using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;

namespace DVLD.Dal.Configurations
{
    internal sealed class TestAppointmenConfig : IEntityTypeConfiguration<TestAppointment>
    {
        public void Configure(EntityTypeBuilder<TestAppointment> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(ta => ta.AppointmentDate)
                .IsRequired();

            builder.Property(ta => ta.PaidFee)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(ta => ta.IsLooked)
                .IsRequired();

            builder.HasOne(x => x.Application)
                .WithMany(x => x.TestAppointments)
                .HasForeignKey(x => x.ApplicationId);

            builder.HasOne(x => x.TestType)
                .WithMany(x => x.TestAppointments)
                .HasForeignKey(x => x.TestTypeId)
                .OnDelete( DeleteBehavior.Restrict);



        }
    }
}
