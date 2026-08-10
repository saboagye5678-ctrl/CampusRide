using MongoDB.Driver;
using Microsoft.Extensions.Options;
using CampusRide.API.Settings;

namespace CampusRide.API.Data;

public class MongoDBService
{
    public IMongoDatabase Database { get; }

    public MongoDBService(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        Database = client.GetDatabase(settings.Value.DatabaseName);
    }
}