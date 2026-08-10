using Microsoft.AspNetCore.Mvc;
using CampusRide.API.Repositories;
using CampusRide.API.Models;
using CampusRide.API.DTOs;

namespace CampusRide.API.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationController : ControllerBase
{
    private readonly LocationRepository _repo;

    public LocationController(LocationRepository repo)
    {
        _repo = repo;
    }

    // GET: all locations
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var locations = await _repo.GetAllAsync();
        return Ok(locations);
    }

    // POST: add location
    [HttpPost]
    public async Task<IActionResult> Create(LocationDto dto)
    {
        var location = new Location
        {
            Name = dto.Name,
            Type = dto.Type,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };

        await _repo.CreateAsync(location);

        return Ok(new { message = "Location added successfully" });
    }
}