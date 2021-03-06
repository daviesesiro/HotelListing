using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly HotelListingContext _context;

    public CountriesRepository(HotelListingContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

    public Task<Country> GetDetails(int id)
    {
        return _context.Countries
            .Include(q => q.Hotels)
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}