using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations;

public class HotelConfigurations : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasData(
            new Hotel { Id = 1, Name = "Hilton", Address = "Geroge town", Rating = 4.5, CountryId = 1 },
            new Hotel { Id = 2, Name = "Sheraton", Address = "Manhattan", Rating = 4.0, CountryId = 3 },
            new Hotel { Id = 3, Name = "Hyatt", Address = "Crazy zone", Rating = 3.5, CountryId = 2 },
            new Hotel { Id = 4, Name = "Marriott", Address = "Match Match street", Rating = 3.0, CountryId = 1 },
            new Hotel { Id = 5, Name = "Four Seasons", Address = "Agege", Rating = 4.0, CountryId = 3 }
        );
    }
}