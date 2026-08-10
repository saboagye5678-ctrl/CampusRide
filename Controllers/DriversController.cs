using Microsoft.AspNetCore.Mvc;
using CampusRide.API.Models;
using CampusRide.API.DTOs;
using CampusRide.API.Repositories;

namespace CampusRide.API.Controllers;

[ApiController]
[Route("api/drivers")]
public class DriversController : ControllerBase
{
    private readonly DriverRepository _repository;

    public DriversController(DriverRepository repository)
    {
        _repository = repository;
    }

    // =========================
    // REGISTER DRIVER
    // =========================
    [HttpPost("register")]
    public async Task<IActionResult> Register(DriverRegisterDto dto)
    {
        var existingDriver =
            await _repository.GetByEmailAsync(dto.Email);

        if (existingDriver != null)
        {
            return BadRequest("Driver already exists");
        }

        var driver = new Driver
        {
            FullName = dto.FullName,
            Email = dto.Email.ToLower(),
            Phone = dto.Phone,
            LicenseNumber = dto.LicenseNumber,
            VehicleNumber = dto.VehicleNumber,
            VehicleType = dto.VehicleType,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Status = "Pending",
            IsOnline = false,

            // Initial location
            Latitude = 0,
            Longitude = 0
        };

        await _repository.CreateAsync(driver);

        return Ok(new
        {
            message = "Driver registration submitted",
            status = "Pending Approval"
        });
    }


    // =========================
    // GET ALL DRIVERS
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAllDrivers()
    {
        var drivers = await _repository.GetAllAsync();
        return Ok(drivers);
    }


    // =========================
    // UPDATE DRIVER LOCATION
    // =========================
    [HttpPut("location")]
    public async Task<IActionResult> UpdateLocation(
        DriverLocationDto dto)
    {
        var driver =
            await _repository.GetByIdAsync(dto.DriverId);

        if (driver == null)
        {
            return NotFound("Driver not found");
        }

        driver.Latitude = dto.Latitude;
        driver.Longitude = dto.Longitude;

        driver.IsOnline = true;

        await _repository.UpdateAsync(
            dto.DriverId,
            driver
        );

        return Ok(new
        {
            message = "Driver location updated"
        });
    }


    // =========================
    // GET DRIVER LOCATION
    // =========================
    [HttpGet("{id}/location")]
    public async Task<IActionResult> GetDriverLocation(string id)
    {
        var driver =
            await _repository.GetByIdAsync(id);

        if (driver == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            latitude = driver.Latitude,
            longitude = driver.Longitude,
            isOnline = driver.IsOnline
        });
    }



    // =========================
    // DRIVER LOGIN
    // =========================
    [HttpPost("login")]
    public async Task<IActionResult> Login(DriverLoginDto dto)
    {
        var driver =
            await _repository.GetByEmailAsync(dto.Email);

        if(driver == null)
        {
            return Unauthorized("Driver not found");
        }


        bool passwordValid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                driver.PasswordHash
            );


        if(!passwordValid)
        {
            return Unauthorized("Invalid password");
        }


        // =========================
        // CHECK DRIVER APPROVAL
        // =========================
        if(driver.Status != "Approved")
        {
            return Unauthorized(new
            {
                message = "Your driver account is waiting for approval"
            });
        }


        return Ok(new
        {
            message = "Login successful",
            driverId = driver.Id,
            name = driver.FullName,
            status = driver.Status
        });
    }



    // =========================
    // SET DRIVER OFFLINE
    // =========================
    [HttpPut("{id}/offline")]
    public async Task<IActionResult> SetOffline(string id)
    {
        var driver =
            await _repository.GetByIdAsync(id);

        if (driver == null)
        {
            return NotFound("Driver not found");
        }

        driver.IsOnline = false;

        await _repository.UpdateAsync(
            id,
            driver
        );

        return Ok(new
        {
            message = "Driver is now offline"
        });
    }
}