namespace CampusRide.API.DTOs;

public class RideBookDto
{
    public string StudentId { get; set; } = string.Empty;

    public string Pickup { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public string StudentPhone { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    // Pickup Coordinates
    public double PickupLatitude { get; set; }

    public double PickupLongitude { get; set; }

    // Destination Coordinates
    public double DestinationLatitude { get; set; }

    public double DestinationLongitude { get; set; }

    public string RideType { get; set; } = string.Empty;
}