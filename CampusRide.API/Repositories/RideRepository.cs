using MongoDB.Driver;
using CampusRide.API.Models;
using CampusRide.API.Data;

namespace CampusRide.API.Repositories;

public class RideRepository
{
    private readonly IMongoCollection<Ride> _rides;


    public RideRepository(MongoDBService mongo)
    {
        _rides = mongo.Database.GetCollection<Ride>("Rides");
    }



    // =========================
    // GET ALL RIDES
    // =========================
    public async Task<List<Ride>> GetAllAsync()
    {
        return await _rides
            .Find(_ => true)
            .ToListAsync();
    }



    // =========================
    // GET RIDE BY ID
    // =========================
    public async Task<Ride?> GetByIdAsync(string id)
    {
        return await _rides
            .Find(r => r.Id == id)
            .FirstOrDefaultAsync();
    }



    // =========================
    // CREATE RIDE
    // =========================
    public async Task CreateAsync(Ride ride)
    {
        await _rides.InsertOneAsync(ride);
    }




    // =========================
    // UPDATE ENTIRE RIDE
    // =========================
    public async Task UpdateAsync(string id, Ride ride)
    {
        await _rides.ReplaceOneAsync(
            r => r.Id == id,
            ride
        );
    }





    // =========================
    // ACCEPT RIDE
    // =========================
    public async Task AcceptRideAsync(
    string rideId,
    string driverId,
    string driverName,
    string driverPhone,
    string vehicleNumber,
    string vehicleType)
{
    var update = Builders<Ride>.Update
        .Set(r => r.Status, "Accepted")
        .Set(r => r.DriverId, driverId)
        .Set(r => r.DriverName, driverName)
        .Set(r => r.DriverPhone, driverPhone)
        .Set(r => r.VehicleNumber, vehicleNumber)
        .Set(r => r.VehicleType, vehicleType)
        .Set(r => r.AcceptedAt, DateTime.UtcNow);

    await _rides.UpdateOneAsync(
        r => r.Id == rideId,
        update
    );
}

// =========================
// DRIVER ARRIVED
// =========================
public async Task DriverArrivedAsync(string rideId)
{
    var update = Builders<Ride>.Update
        .Set(r => r.Status, "Arrived")
        .Set(r => r.ArrivedAt, DateTime.UtcNow);

    await _rides.UpdateOneAsync(
        r => r.Id == rideId,
        update
    );
}

    // =========================
    // START RIDE
    // =========================
    public async Task StartRideAsync(string rideId)
    {

        var update = Builders<Ride>.Update

            .Set(r => r.Status, "InProgress")

            .Set(r => r.StartedAt, DateTime.UtcNow);



        await _rides.UpdateOneAsync(
            r => r.Id == rideId,
            update
        );

    }





    // =========================
    // COMPLETE RIDE
    // =========================
    public async Task CompleteRideAsync(string rideId)
    {

        var update = Builders<Ride>.Update

            .Set(r => r.Status, "Completed")

            .Set(r => r.CompletedAt, DateTime.UtcNow);



        await _rides.UpdateOneAsync(
            r => r.Id == rideId,
            update
        );

    }





    // =========================
    // CANCEL RIDE
    // =========================
    public async Task CancelRideAsync(string rideId)
    {

        var update = Builders<Ride>.Update

            .Set(r => r.Status, "Cancelled");



        await _rides.UpdateOneAsync(
            r => r.Id == rideId,
            update
        );

    }





    // =========================
    // GET PENDING RIDES
    // =========================
    public async Task<List<Ride>> GetPendingRidesAsync()
    {

        return await _rides

            .Find(r => r.Status == "Searching")

            .ToListAsync();

    }





    // =========================
    // GET DRIVER ACTIVE RIDE
    // =========================
    public async Task<Ride?> GetDriverActiveRideAsync(string driverId)
    {

        return await _rides

            .Find(r =>

                r.DriverId == driverId &&

                (
                    r.Status == "Accepted" ||

                    r.Status == "Arrived" ||

                    r.Status == "InProgress"
                )

            )

            .FirstOrDefaultAsync();

    }

}