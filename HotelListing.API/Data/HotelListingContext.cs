using Microsoft.EntityFrameworkCore;
namespace HotelListing.API.Data;

public class HotelListingContext : DbContext
{
    public HotelListingContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Country> Countries { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasData(
            new Country { Id = 1, Name = "USA", ShortName = "US" },
            new Country { Id = 2, Name = "Canada", ShortName = "CA" },
            new Country { Id = 3, Name = "Mexico", ShortName = "MX" }
        );

        modelBuilder.Entity<Hotel>().HasData(
            new Hotel { Id = 1, Name = "Hilton", Address = "Geroge town", Rating = 4.5, CountryId = 1 },
            new Hotel { Id = 2, Name = "Sheraton", Address = "Manhattan", Rating = 4.0, CountryId = 3 },
            new Hotel { Id = 3, Name = "Hyatt", Address = "Crazy zone", Rating = 3.5, CountryId = 2 },
            new Hotel { Id = 4, Name = "Marriott", Address = "Match Match street", Rating = 3.0, CountryId = 1 },
            new Hotel { Id = 5, Name = "Four Seasons", Address = "Agege", Rating = 4.0, CountryId = 3 }
        );
    }


}

