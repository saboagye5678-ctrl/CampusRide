namespace CampusRide.API.DTOs;

public class StudentNoticeDto
{
    public string UserId { get; set; }

    public string LocationId { get; set; }

    public string DestinationLocationId { get; set; }

    public string ClassTime { get; set; }

    public int StudentCount { get; set; }
}

public class DriverAvailabilityDto
{
    public string UserId { get; set; }

    public string LocationId { get; set; }

    public string AvailableTime { get; set; }

    public string VehicleType { get; set; }

    public int AvailableSeats { get; set; }
}