namespace CampusRide.API.DTOs;

public class DriverLocationDto
{
    public string DriverId { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}