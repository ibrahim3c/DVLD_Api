using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Dal.Configurations
{
    internal sealed class TestConfig : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Notes)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(x => x.TestAppointment)
                .WithOne(x => x.Test)
                .HasForeignKey<Test>(t => t.TestAppointmentId);
        }
    }
}
