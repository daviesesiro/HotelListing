using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations;

public class CountryConfigurations : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(
            new Country { Id = 1, Name = "USA", ShortName = "US" },
            new Country { Id = 2, Name = "Canada", ShortName = "CA" },
            new Country { Id = 3, Name = "Mexico", ShortName = "MX" }
        );
    }
}