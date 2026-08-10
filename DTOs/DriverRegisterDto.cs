namespace CampusRide.API.DTOs;

public class DriverRegisterDto
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public string VehicleNumber { get; set; } = string.Empty;

    public string VehicleType { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}