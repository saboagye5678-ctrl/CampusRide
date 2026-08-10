using Microsoft.AspNetCore.Mvc;
using CampusRide.API.Repositories;
using CampusRide.API.Models;
using CampusRide.API.DTOs;

namespace CampusRide.API.Controllers;

[ApiController]
[Route("api/notices")]
public class NoticeController : ControllerBase
{
    private readonly NoticeRepository _noticeRepo;
    private readonly LocationRepository _locationRepo;

    public NoticeController(
        NoticeRepository noticeRepo,
        LocationRepository locationRepo)
    {
        _noticeRepo = noticeRepo;
        _locationRepo = locationRepo;
    }

    // =========================
    // STUDENT POST NOTICE
    // =========================
    [HttpPost("student")]
    public async Task<IActionResult> CreateStudentNotice(StudentNoticeDto dto)
    {
        // Find pickup location from existing Locations collection
        var locations = await _locationRepo.GetAllAsync();

        var pickupLocation = locations
            .FirstOrDefault(l => l.Id == dto.LocationId);

        if (pickupLocation == null)
        {
            return BadRequest("Selected pickup location does not exist.");
        }

        // Find destination from existing Locations collection
        var destinationLocation = locations
            .FirstOrDefault(l => l.Id == dto.DestinationLocationId);

        if (destinationLocation == null)
        {
            return BadRequest("Selected destination location does not exist.");
        }

        var notice = new TransportNotice
        {
            Type = "StudentNotice",

            UserId = dto.UserId,

            LocationId = pickupLocation.Id,
            LocationName = pickupLocation.Name,

            Latitude = pickupLocation.Latitude,
            Longitude = pickupLocation.Longitude,

            ClassTime = dto.ClassTime,

            DestinationLocationId = destinationLocation.Id,
            DestinationLocationName = destinationLocation.Name,

            StudentCount = dto.StudentCount,

            CreatedAt = DateTime.UtcNow
        };

        await _noticeRepo.CreateAsync(notice);

        return Ok(new
        {
            message = "Student transport notice posted successfully",
            notice
        });
    }


    // =========================
    // DRIVER POST AVAILABILITY
    // =========================
    [HttpPost("driver")]
    public async Task<IActionResult> CreateDriverAvailability(
        DriverAvailabilityDto dto)
    {
        // Find location from existing Locations collection
        var locations = await _locationRepo.GetAllAsync();

        var location = locations
            .FirstOrDefault(l => l.Id == dto.LocationId);

        if (location == null)
        {
            return BadRequest("Selected location does not exist.");
        }

        var notice = new TransportNotice
        {
            Type = "DriverAvailability",

            UserId = dto.UserId,

            LocationId = location.Id,
            LocationName = location.Name,

            Latitude = location.Latitude,
            Longitude = location.Longitude,

            AvailableTime = dto.AvailableTime,

            VehicleType = dto.VehicleType,

            AvailableSeats = dto.AvailableSeats,

            CreatedAt = DateTime.UtcNow
        };

        await _noticeRepo.CreateAsync(notice);

        return Ok(new
        {
            message = "Driver availability posted successfully",
            notice
        });
    }


    // =========================
    // GET NOTICES FOR DRIVERS
    // =========================
    [HttpGet("for-drivers")]
    public async Task<IActionResult> GetForDrivers()
    {
        var notices = await _noticeRepo.GetStudentNoticesAsync();

        return Ok(notices);
    }


    // =========================
    // GET NOTICES FOR STUDENTS
    // =========================
    [HttpGet("for-students")]
    public async Task<IActionResult> GetForStudents()
    {
        var notices = await _noticeRepo.GetDriverAvailabilityAsync();

        return Ok(notices);
    }


    // =========================
    // GET ALL NOTICES
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notices = await _noticeRepo.GetAllAsync();

        return Ok(notices);
    }
}