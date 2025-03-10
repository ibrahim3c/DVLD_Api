using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Dal.Configurations
{
    internal sealed class LicenseClassConfig : IEntityTypeConfiguration<LicenseClass>
    {
        public void Configure(EntityTypeBuilder<LicenseClass> builder)
        {
            builder.ToTable("LicenseClasses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(x => x.Fee)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasData(
   new LicenseClass { Id = 1, Name = "Class 1 - Small Motorcycle",        Description = "It allows the driver to drive small motorcycles.", MinAge = 18,  ValidityPeriod= 5, Fee = 15.00m },
        new LicenseClass { Id = 2, Name = "Class 2 - Heavy Motorcycle License",Description = "Heavy Motorcycle License (Large Motorcycle).",MinAge = 21,       ValidityPeriod= 5, Fee = 30.00m },
        new LicenseClass { Id = 3, Name = "Class 3 - Ordinary driving license",Description = "Ordinary driving license (car licence).", MinAge = 18,           ValidityPeriod= 10, Fee = 20.00m },
        new LicenseClass { Id = 4, Name = "Class 4 - Commercial",              Description = "Commercial driving license (taxi/limousine).", MinAge = 21,      ValidityPeriod = 10,Fee = 200.00m },
        new LicenseClass { Id = 5, Name = "Class 5 - Agricultural",            Description = "Agricultural and work vehicles used in farming.", MinAge = 21,   ValidityPeriod = 10, Fee = 50.00m },
        new LicenseClass { Id = 6, Name = "Class 6 - Small and medium bus",    Description = "Small and medium bus license.", MinAge = 21,                     ValidityPeriod= 10, Fee = 250.00m },
        new LicenseClass { Id = 7, Name = "Class 7 - Truck and heavy vehicle", Description = "Truck and heavy vehicle license.", MinAge = 21,                  ValidityPeriod= 10, Fee = 300.00m }
       );
        }
    }
}
