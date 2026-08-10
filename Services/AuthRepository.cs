using MongoDB.Driver;
using CampusRide.API.Models;
using CampusRide.API.DTOs;
using CampusRide.API.Data;

namespace CampusRide.API.Services;

public class AuthRepository
{
    private readonly IMongoCollection<User> _users;

    public AuthRepository(MongoDBService mongo)
    {
        _users = mongo.Database.GetCollection<User>("Users");
    }

    // CHECK IF USER EXISTS
    public async Task<bool> UserExists(string email)
    {
        return await _users
            .Find(x => x.Email == email.ToLower())
            .AnyAsync();
    }

    // REGISTER USER
    public async Task<User> Register(RegisterDto dto)
    {
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email.ToLower(),
            StudentId = dto.StudentId,
            Phone = dto.Phone,

            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(dto.Password),

            Role = "Student"
        };

        await _users.InsertOneAsync(user);

        return user;
    }

    // LOGIN USER
    public async Task<User> Login(string email, string password)
    {
        var user = await _users
            .Find(x => x.Email == email.ToLower())
            .FirstOrDefaultAsync();

        if (user == null)
            return null;

        bool valid =
            BCrypt.Net.BCrypt.Verify(
                password,
                user.PasswordHash
            );

        return valid ? user : null;
    }
}

