using Microsoft.AspNetCore.Mvc;
using CampusRide.API.DTOs;
using CampusRide.API.Models;
using CampusRide.API.Repositories;
using CampusRide.API.Data;
using MongoDB.Driver;

namespace CampusRide.API.Controllers;

[ApiController]
[Route("api/rides")]
public class RidesController : ControllerBase
{

    private readonly RideRepository _rideRepo;
    private readonly IMongoCollection<Driver> _drivers;


    public RidesController(
    RideRepository rideRepo,
    MongoDBService mongo)
{
    _rideRepo = rideRepo;
    _drivers = mongo.Database.GetCollection<Driver>("Drivers");
}



    // =========================
    // BOOK RIDE (STUDENT)
    // =========================

    [HttpPost("book")]
    public async Task<IActionResult> BookRide(RideBookDto dto)
    {

        if(string.IsNullOrEmpty(dto.Pickup) ||
           string.IsNullOrEmpty(dto.Destination))
        {
            return BadRequest(
                "Pickup and destination required"
            );
        }



        var fare = CalculateFare(
    dto.PickupLatitude,
    dto.PickupLongitude,
    dto.DestinationLatitude,
    dto.DestinationLongitude
);



       var ride = new Ride
{
    // STUDENT INFORMATION
    StudentId = dto.StudentId,
    StudentName = dto.StudentName,
    StudentPhone = dto.StudentPhone,

    // TRIP INFORMATION
    Pickup = dto.Pickup,
    PickupLatitude = dto.PickupLatitude,
    PickupLongitude = dto.PickupLongitude,

    Destination = dto.Destination,
    DestinationLatitude = dto.DestinationLatitude,
    DestinationLongitude = dto.DestinationLongitude,

    RideType = dto.RideType,

    // PAYMENT
    Fare = fare,

    // STATUS
    Status = "Searching",

    // TIME
    CreatedAt = DateTime.UtcNow
};

        await _rideRepo.CreateAsync(ride);



        return Ok(new
        {

            id = ride.Id,

            message =
            "Searching for nearby drivers",

            fare,

            status = ride.Status

        });

    }






    // =========================
    // GET ALL RIDES
    // =========================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        var rides =
            await _rideRepo.GetAllAsync();


        return Ok(rides);

    }






    // =========================
    // GET SINGLE RIDE
    // =========================

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRide(string id)
    {

        var ride =
            await _rideRepo.GetByIdAsync(id);



        if(ride == null)
        {
            return NotFound();
        }



        return Ok(ride);

    }







    // =========================
    // DRIVER AVAILABLE RIDES
    // =========================

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableRides()
    {

        var rides =
            await _rideRepo.GetAllAsync();



        var available =
            rides
            .Where(r =>
                r.Status == "Searching"
            )
            .ToList();



        return Ok(available);

    }







    // =========================
// DRIVER ACCEPT RIDE
// =========================

[HttpPut("{id}/accept")]
public async Task<IActionResult> AcceptRide(
    string id,
    DriverAcceptDto dto)
{
    var ride = await _rideRepo.GetByIdAsync(id);

    if (ride == null)
    {
        return NotFound("Ride not found");
    }

    if (ride.Status != "Searching")
    {
        return BadRequest("Ride is no longer available");
    }

    // Find driver
    var driver = await _drivers
        .Find(d => d.Id == dto.DriverId)
        .FirstOrDefaultAsync();

    if (driver == null)
    {
        return NotFound("Driver not found");
    }

    // Accept ride and save driver information
    await _rideRepo.AcceptRideAsync(
        id,
        driver.Id,
        driver.FullName,
        driver.Phone,
        driver.VehicleNumber,
        driver.VehicleType
    );

    return Ok(new
    {
        message = "Ride accepted successfully",
        rideId = id,
        status = "Accepted",
        driverName = driver.FullName,
        driverPhone = driver.Phone,
        vehicleNumber = driver.VehicleNumber,
        vehicleType = driver.VehicleType
    });
}







    // =========================
    // DRIVER ARRIVED
    // =========================

    [HttpPut("{id}/arrive")]
    public async Task<IActionResult> DriverArrived(
        string id)
    {


        var ride =
            await _rideRepo.GetByIdAsync(id);



        if(ride == null)
        {
            return NotFound(
                "Ride not found"
            );
        }




        if(ride.Status != "Accepted")
        {
            return BadRequest(
                "Driver must accept ride first"
            );
        }





        await _rideRepo.DriverArrivedAsync(id);



        return Ok(new
        {

            message =
            "Driver has arrived",

            status =
            "Arrived"

        });


    }








    // =========================
    // START RIDE
    // =========================

    [HttpPut("{id}/start")]
    public async Task<IActionResult> StartRide(
        string id)
    {


        var ride =
            await _rideRepo.GetByIdAsync(id);



        if(ride == null)
        {
            return NotFound(
                "Ride not found"
            );
        }



        if(ride.Status != "Arrived")
        {
            return BadRequest(
                "Driver has not arrived"
            );
        }




        await _rideRepo.StartRideAsync(id);




        return Ok(new
        {

            message =
            "Ride started",

            status =
            "InProgress"

        });


    }








    // =========================
    // COMPLETE RIDE
    // =========================

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteRide(
        string id)
    {


        var ride =
            await _rideRepo.GetByIdAsync(id);



        if(ride == null)
        {
            return NotFound(
                "Ride not found"
            );
        }




        if(ride.Status != "InProgress")
        {
            return BadRequest(
                "Ride is not active"
            );
        }





        await _rideRepo.CompleteRideAsync(id);




        return Ok(new
        {

            message =
            "Ride completed successfully",

            status =
            "Completed"

        });


    }









    // =========================
// STUDENT CHECK STATUS
// =========================

[HttpGet("{id}/status")]
public async Task<IActionResult> GetRideStatus(string id)
{
    var ride = await _rideRepo.GetByIdAsync(id);

    if (ride == null)
    {
        return NotFound("Ride not found");
    }

    return Ok(new
    {
        status = ride.Status,

        driverId = ride.DriverId,
        driverName = ride.DriverName,
        driverPhone = ride.DriverPhone,
        vehicleNumber = ride.VehicleNumber,
        vehicleType = ride.VehicleType
    });
}

// =========================
// GET DRIVER ACTIVE RIDE
// =========================

[HttpGet("driver/{driverId}/active")]
public async Task<IActionResult> GetDriverActiveRide(
    string driverId)
{

    var ride =
        await _rideRepo.GetDriverActiveRideAsync(driverId);


    if(ride == null)
    {
        return NotFound(
            "No active ride"
        );
    }


    return Ok(ride);

}




  // =========================
// FARE CALCULATION
// =========================

private decimal CalculateFare(
    double pickupLat,
    double pickupLng,
    double destinationLat,
    double destinationLng)
{

    double distance = CalculateDistance(
        pickupLat,
        pickupLng,
        destinationLat,
        destinationLng
    );


    if(distance <= 5)
    {
        return 20;
    }

    else if(distance <= 7)
    {
        return 23;
    }

    else if(distance <= 10)
    {
        return 25;
    }

    else if(distance <= 15)
    {
        return 30;
    }

    else if(distance <= 20)
    {
        return 40;
    }

    else
    {
        // For distances above 20km
        return 40;
    }

}



// =========================
// DISTANCE CALCULATION
// =========================

private double CalculateDistance(
    double lat1,
    double lon1,
    double lat2,
    double lon2)
{

    const double earthRadius = 6371;


    double dLat =
        (lat2 - lat1) * Math.PI / 180;


    double dLon =
        (lon2 - lon1) * Math.PI / 180;


    double a =
        Math.Sin(dLat / 2) *
        Math.Sin(dLat / 2)
        +
        Math.Cos(lat1 * Math.PI / 180)
        *
        Math.Cos(lat2 * Math.PI / 180)
        *
        Math.Sin(dLon / 2)
        *
        Math.Sin(dLon / 2);


    double c =
        2 *
        Math.Atan2(
            Math.Sqrt(a),
            Math.Sqrt(1 - a)
        );


    double distance =
        earthRadius * c;


    return distance;

}

// =========================
// STUDENT CANCEL RIDE
// =========================

[HttpPut("{id}/cancel")]
public async Task<IActionResult> CancelRide(string id)
{
    var ride = await _rideRepo.GetByIdAsync(id);

    if (ride == null)
    {
        return NotFound("Ride not found");
    }

    // Cancellation allowed only before driver arrives
    if (ride.Status != "Searching" &&
        ride.Status != "Accepted")
    {
        return BadRequest(
            $"Ride cannot be cancelled when status is {ride.Status}"
        );
    }

    await _rideRepo.CancelRideAsync(id);

    return Ok(new
    {
        message = "Ride cancelled successfully",
        rideId = id,
        status = "Cancelled"
    });
}
}

