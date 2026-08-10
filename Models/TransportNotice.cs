namespace CampusRide.API.Models;

public class TransportNotice
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // StudentNotice or DriverAvailability
    public string Type { get; set; }

    // User who created the notice
    public string UserId { get; set; }

    // Location selected from the existing Locations collection
    public string LocationId { get; set; }
    public string LocationName { get; set; }

    // Coordinates copied from the selected Location
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Student notice information
    public string? ClassTime { get; set; }
    public string? DestinationLocationId { get; set; }
    public string? DestinationLocationName { get; set; }
    public int? StudentCount { get; set; }

    // Driver availability information
    public string? AvailableTime { get; set; }
    public string? VehicleType { get; set; }
    public int? AvailableSeats { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}