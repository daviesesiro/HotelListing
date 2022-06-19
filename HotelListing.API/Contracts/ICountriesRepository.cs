using HotelListing.API.Data;

namespace HotelListing.API.Contracts;

public interface ICountriesRepository : IGenericRespository<Country>
{
    Task<Country> GetDetails(int id);
}