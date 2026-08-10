namespace CampusRide.API.Models;

public class Location
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Type { get; set; } // BusStop, TaxiPoint, Hostel, etc.
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}