using DVLD.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD.Dal.Configurations
{
    internal sealed class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(GetAllCountries());
        }
        private static List<Country> GetAllCountries()
        {
            return new List<Country>
        {
            new Country { CountryId = 1, CountryName = "Afghanistan" },
            new Country { CountryId = 2, CountryName = "Albania" },
            new Country { CountryId = 3, CountryName = "Algeria" },
            new Country { CountryId = 4, CountryName = "Andorra" },
            new Country { CountryId = 5, CountryName = "Angola" },
            new Country { CountryId = 6, CountryName = "Argentina" },
            new Country { CountryId = 7, CountryName = "Armenia" },
            new Country { CountryId = 8, CountryName = "Australia" },
            new Country { CountryId = 9, CountryName = "Austria" },
            new Country { CountryId = 10, CountryName = "Azerbaijan" },
            new Country { CountryId = 11, CountryName = "Bahamas" },
            new Country { CountryId = 12, CountryName = "Bahrain" },
            new Country { CountryId = 13, CountryName = "Bangladesh" },
            new Country { CountryId = 14, CountryName = "Barbados" },
            new Country { CountryId = 15, CountryName = "Belarus" },
            new Country { CountryId = 16, CountryName = "Belgium" },
            new Country { CountryId = 17, CountryName = "Belize" },
            new Country { CountryId = 18, CountryName = "Benin" },
            new Country { CountryId = 19, CountryName = "Bhutan" },
            new Country { CountryId = 20, CountryName = "Bolivia" },
            new Country { CountryId = 21, CountryName = "Bosnia and Herzegovina" },
            new Country { CountryId = 22, CountryName = "Botswana" },
            new Country { CountryId = 23, CountryName = "Brazil" },
            new Country { CountryId = 24, CountryName = "Brunei" },
            new Country { CountryId = 25, CountryName = "Bulgaria" },
            new Country { CountryId = 26, CountryName = "Burkina Faso" },
            new Country { CountryId = 27, CountryName = "Burundi" },
            new Country { CountryId = 28, CountryName = "Cambodia" },
            new Country { CountryId = 29, CountryName = "Cameroon" },
            new Country { CountryId = 30, CountryName = "Canada" },
            new Country { CountryId = 31, CountryName = "Cape Verde" },
            new Country { CountryId = 32, CountryName = "Central African Republic" },
            new Country { CountryId = 33, CountryName = "Chad" },
            new Country { CountryId = 34, CountryName = "Chile" },
            new Country { CountryId = 35, CountryName = "China" },
            new Country { CountryId = 36, CountryName = "Colombia" },
            new Country { CountryId = 37, CountryName = "Comoros" },
            new Country { CountryId = 38, CountryName = "Congo" },
            new Country { CountryId = 39, CountryName = "Costa Rica" },
            new Country { CountryId = 40, CountryName = "Croatia" },
            new Country { CountryId = 41, CountryName = "Cuba" },
            new Country { CountryId = 42, CountryName = "Cyprus" },
            new Country { CountryId = 43, CountryName = "Czech Republic" },
            new Country { CountryId = 44, CountryName = "Denmark" },
            new Country { CountryId = 45, CountryName = "Djibouti" },
            new Country { CountryId = 46, CountryName = "Dominica" },
            new Country { CountryId = 47, CountryName = "Dominican Republic" },
            new Country { CountryId = 48, CountryName = "Ecuador" },
            new Country { CountryId = 49, CountryName = "Egypt" },
            new Country { CountryId = 50, CountryName = "El Salvador" },
            new Country { CountryId = 51, CountryName = "Equatorial Guinea" },
            new Country { CountryId = 52, CountryName = "Eritrea" },
            new Country { CountryId = 53, CountryName = "Estonia" },
            new Country { CountryId = 54, CountryName = "Eswatini" },
            new Country { CountryId = 55, CountryName = "Ethiopia" },
            new Country { CountryId = 56, CountryName = "Fiji" },
            new Country { CountryId = 57, CountryName = "Finland" },
            new Country { CountryId = 58, CountryName = "France" },
            new Country { CountryId = 59, CountryName = "Gabon" },
            new Country { CountryId = 60, CountryName = "Gambia" },
            new Country { CountryId = 61, CountryName = "Georgia" },
            new Country { CountryId = 62, CountryName = "Germany" },
            new Country { CountryId = 63, CountryName = "Ghana" },
            new Country { CountryId = 64, CountryName = "Greece" },
            new Country { CountryId = 65, CountryName = "Grenada" },
            new Country { CountryId = 66, CountryName = "Guatemala" },
            new Country { CountryId = 67, CountryName = "Guinea" },
            new Country { CountryId = 68, CountryName = "Guinea-Bissau" },
            new Country { CountryId = 69, CountryName = "Guyana" },
            new Country { CountryId = 70, CountryName = "Haiti" },
            new Country { CountryId = 71, CountryName = "Honduras" },
            new Country { CountryId = 72, CountryName = "Hungary" },
            new Country { CountryId = 73, CountryName = "Iceland" },
            new Country { CountryId = 74, CountryName = "India" },
            new Country { CountryId = 75, CountryName = "Indonesia" },
            new Country { CountryId = 76, CountryName = "Iran" },
            new Country { CountryId = 77, CountryName = "Iraq" },
            new Country { CountryId = 78, CountryName = "Ireland" },
            new Country { CountryId = 79, CountryName = "Israel" },
            new Country { CountryId = 80, CountryName = "Italy" },
            new Country { CountryId = 81, CountryName = "Jamaica" },
            new Country { CountryId = 82, CountryName = "Japan" }
        };
        }

    }
}
