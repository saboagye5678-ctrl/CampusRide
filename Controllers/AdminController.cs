using Microsoft.AspNetCore.Mvc;
using CampusRide.API.Repositories;

namespace CampusRide.API.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly DriverRepository _driverRepo;

    public AdminController(DriverRepository driverRepo)
    {
        _driverRepo = driverRepo;
    }

    // GET all pending drivers
    [HttpGet("pending-drivers")]
    public async Task<IActionResult> GetPendingDrivers()
    {
        var all = await _driverRepo.GetAllAsync();

        var pending = all.Where(d => d.Status == "Pending");

        return Ok(pending);
    }

    // Approve driver
    [HttpPost("approve-driver/{id}")]
    public async Task<IActionResult> ApproveDriver(string id)
    {
        await _driverRepo.ApproveDriver(id);
        return Ok("Driver approved successfully");
    }

    // Reject driver
    [HttpPost("reject-driver/{id}")]
    public async Task<IActionResult> RejectDriver(string id)
    {
        await _driverRepo.RejectDriver(id);
        return Ok("Driver rejected");
    }
}