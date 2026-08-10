using MongoDB.Driver;
using CampusRide.API.Models;
using CampusRide.API.Data;

namespace CampusRide.API.Repositories;

public class NoticeRepository
{
    private readonly IMongoCollection<TransportNotice> _notices;

    public NoticeRepository(MongoDBService mongo)
    {
        _notices = mongo.Database.GetCollection<TransportNotice>("TransportNotices");
    }

    // =========================
    // CREATE NOTICE
    // =========================
    public async Task CreateAsync(TransportNotice notice)
    {
        await _notices.InsertOneAsync(notice);
    }

    // =========================
    // GET ALL NOTICES
    // =========================
    public async Task<List<TransportNotice>> GetAllAsync()
    {
        return await _notices
            .Find(_ => true)
            .SortByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    // =========================
    // GET STUDENT NOTICES
    // =========================
    public async Task<List<TransportNotice>> GetStudentNoticesAsync()
    {
        return await _notices
            .Find(n => n.Type == "StudentNotice")
            .SortBy(n => n.ClassTime)
            .ToListAsync();
    }

    // =========================
    // GET DRIVER AVAILABILITY
    // =========================
    public async Task<List<TransportNotice>> GetDriverAvailabilityAsync()
    {
        return await _notices
            .Find(n => n.Type == "DriverAvailability")
            .SortBy(n => n.AvailableTime)
            .ToListAsync();
    }
}