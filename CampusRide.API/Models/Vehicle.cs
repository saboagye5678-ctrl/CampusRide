using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampusRide.API.Models;

public class Vehicle
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string VehicleCode { get; set; } = string.Empty;

    public string VehicleName { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string PlateNumber { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public string DriverId { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}