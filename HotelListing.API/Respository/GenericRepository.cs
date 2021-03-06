using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly HotelListingContext _context;
    private readonly IMapper _mapper;

    public GenericRepository(HotelListingContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<T> GetAsync(int? id)
    {
        if (id is null) return null;

        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters q)
    {
        var totalSize = await _context.Set<T>().CountAsync();
        var items = await _context.Set<T>()
            .Skip(q.StartIndex)
            .Take(q.PageSize)
            .ProjectTo<TResult>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<TResult>
        {
            Items = items,
            PageNumber = q.PageNumber,
            RecordNumber = q.PageSize,
            TotalCount = totalSize,
        };
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id)
    {
        var entity = await GetAsync(id);
        return entity != null;
    }
}