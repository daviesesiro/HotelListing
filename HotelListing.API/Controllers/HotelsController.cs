using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Hotel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IHotelsRepository _hotelsRepository;
    private readonly IMapper _mapper;

    public HotelsController(IHotelsRepository hotelsRepository, IMapper mapper)
    {
        _hotelsRepository = hotelsRepository;
        _mapper = mapper;
    }


    // GET: api/Hotels
    [HttpGet]

    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
    {
        var hotels = await _hotelsRepository.GetAllAsync();
        return Ok(_mapper.Map<List<HotelDto>>(hotels));
    }

    // GET: api/Hotels/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        var hotel = await _hotelsRepository.GetAsync(id);

        if (hotel == null) return NotFound();

        return Ok(_mapper.Map<HotelDto>(hotel));
    }

    // PUT: api/Hotels/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutHotel(int id, HotelDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var hotel = await _hotelsRepository.GetAsync(id);
        if (hotel == null) return NotFound();

        _mapper.Map(dto, hotel);

        try
        {
            await _hotelsRepository.UpdateAsync(hotel);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await HotelExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Hotels
    [HttpPost]
    public async Task<ActionResult<HotelDto>> PostHotel(CreateHotelDto dto)
    {
        var hotel = _mapper.Map<Hotel>(dto);
        var record = await _hotelsRepository.AddAsync(hotel);

        return CreatedAtAction("GetHotel", new { id = hotel.Id }, _mapper.Map<HotelDto>(record));
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await _hotelsRepository.GetAsync(id);
        if (hotel == null) return NotFound();

        await _hotelsRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> HotelExists(int id)
    {
        return await _hotelsRepository.Exists(id);
    }
}