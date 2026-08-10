using Microsoft.AspNetCore.Mvc;
using CampusRide.API.DTOs;
using CampusRide.API.Services;

namespace CampusRide.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthRepository _repo;
    private readonly IConfiguration _config;

    public AuthController(
        AuthRepository repo,
        IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    // -------------------------
    // REGISTER
    // -------------------------

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.StudentId) ||
            string.IsNullOrWhiteSpace(dto.Phone) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest("All fields are required");
        }

        // Check if user exists
        if (await _repo.UserExists(dto.Email))
        {
            return BadRequest("User already exists");
        }

        // Create user
        var user = await _repo.Register(dto);

        return Ok(new
        {
            message = "Account created successfully",
            user.Email
        });
    }

    // -------------------------
    // LOGIN
    // -------------------------

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user =
            await _repo.Login(
                dto.Email,
                dto.Password
            );

        if (user == null)
        {
            return BadRequest(
                "Invalid email or password"
            );
        }

        // Simple token
        var token =
            Guid.NewGuid().ToString();

        return Ok(new
        {
            token,

            role = user.Role,

            fullName = user.FullName,

            email = user.Email,

            studentId = user.StudentId,

            phone = user.Phone
        });
    }
}