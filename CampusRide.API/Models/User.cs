
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampusRide.API.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string StudentId { get; set; }

    public string Phone { get; set; }

    public string PasswordHash { get; set; }

    public string Role { get; set; }
}

