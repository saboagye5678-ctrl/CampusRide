using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampusRide.API.Models;

public class Driver
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public string VehicleNumber { get; set; } = string.Empty;

    public string VehicleType { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public bool IsOnline { get; set; } = false;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}