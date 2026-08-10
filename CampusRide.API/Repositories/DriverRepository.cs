using MongoDB.Driver;
using CampusRide.API.Models;
using CampusRide.API.Data;

namespace CampusRide.API.Repositories;

public class DriverRepository
{
    private readonly IMongoCollection<Driver> _drivers;

    public DriverRepository(MongoDBService mongo)
    {
        _drivers = mongo.Database.GetCollection<Driver>("Drivers");
    }

    // =========================
    // GET ALL DRIVERS
    // =========================
    public async Task<List<Driver>> GetAllAsync()
    {
        return await _drivers.Find(_ => true).ToListAsync();
    }

    // =========================
    // GET DRIVER BY EMAIL
    // =========================
    public async Task<Driver?> GetByEmailAsync(string email)
    {
        return await _drivers
            .Find(d => d.Email == email.ToLower())
            .FirstOrDefaultAsync();
    }

    // =========================
    // GET DRIVER BY ID
    // =========================
    public async Task<Driver?> GetByIdAsync(string id)
    {
        return await _drivers
            .Find(d => d.Id == id)
            .FirstOrDefaultAsync();
    }

    // =========================
    // CREATE DRIVER
    // =========================
    public async Task CreateAsync(Driver driver)
    {
        await _drivers.InsertOneAsync(driver);
    }

    // =========================
    // UPDATE DRIVER
    // =========================
    public async Task UpdateAsync(string id, Driver driver)
    {
        await _drivers.ReplaceOneAsync(d => d.Id == id, driver);
    }

    // =========================
    // APPROVE DRIVER
    // =========================
    public async Task ApproveDriver(string id)
    {
        var update = Builders<Driver>.Update
            .Set(d => d.Status, "Approved");

        await _drivers.UpdateOneAsync(
            d => d.Id == id,
            update
        );
    }

    // =========================
    // REJECT DRIVER
    // =========================
    public async Task RejectDriver(string id)
    {
        var update = Builders<Driver>.Update
            .Set(d => d.Status, "Rejected");

        await _drivers.UpdateOneAsync(
            d => d.Id == id,
            update
        );
    }
}