using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampusRide.API.Models;

public class Stop
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}