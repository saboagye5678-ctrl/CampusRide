using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampusRide.API.Models;

public class Ride
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }


    // STUDENT INFORMATION

    public string StudentId { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public string StudentPhone { get; set; } = string.Empty;

    // DRIVER INFORMATION

    public string? DriverId { get; set; }

    public string? DriverName { get; set; }

    public string? DriverPhone { get; set; }

    public string? VehicleNumber { get; set; }

public string? VehicleType { get; set; }



    // TRIP INFORMATION

    public string Pickup { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    // PICKUP GPS
public double PickupLatitude { get; set; }

public double PickupLongitude { get; set; }

// DESTINATION GPS
public double DestinationLatitude { get; set; }

public double DestinationLongitude { get; set; }


    public string RideType { get; set; } = string.Empty;



    // PAYMENT

    public decimal Fare { get; set; }



    // STATUS

    // Searching
    // Accepted
    // Arrived
    // InProgress
    // Completed
    // Cancelled

    public string Status { get; set; } = "Searching";



    // TIME TRACKING

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? AcceptedAt { get; set; }

    public DateTime? ArrivedAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}