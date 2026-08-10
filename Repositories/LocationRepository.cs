using MongoDB.Driver;
using CampusRide.API.Models;
using CampusRide.API.Data;

namespace CampusRide.API.Repositories;

public class LocationRepository
{
    private readonly IMongoCollection<Location> _locations;

    public LocationRepository(MongoDBService mongo)
    {
        _locations = mongo.Database.GetCollection<Location>("Locations");
    }

    // GET ALL LOCATIONS
    public async Task<List<Location>> GetAllAsync()
    {
        return await _locations.Find(_ => true).ToListAsync();
    }

    // ADD LOCATION
    public async Task CreateAsync(Location location)
    {
        await _locations.InsertOneAsync(location);
    }
}