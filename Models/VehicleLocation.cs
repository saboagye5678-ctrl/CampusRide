using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampusRide.API.Models;

public class VehicleLocation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string VehicleId { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double Speed { get; set; }

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}