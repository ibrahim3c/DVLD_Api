using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Dal.Configurations
{
    internal sealed class TestTypeConfig : IEntityTypeConfiguration<TestType>
    {
        public void Configure(EntityTypeBuilder<TestType> builder)
        {
            builder.ToTable("TestTypes");

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
            new TestType
            {
                Id = 1,
                Title = "Vision Test",
                Description = "This assesses the applicant's visual acuity to ensure they meet the minimum vision standards.",
                TypeFee = 10.00000M
            },
            new TestType
            {
                Id = 2,
                Title = "Written (Theory) Test",
                Description = "This test assesses the applicant's knowledge of traffic laws, road signs, and safe driving practices.",
                TypeFee = 20.00000M
            },
            new TestType
            {
                Id = 3,
                Title = "Practical (Street) Test",
                Description = "This test evaluates the applicant's driving skills and ability to operate a vehicle safely.",
                TypeFee = 35.00000M
            }
        );
        }
    }
}
