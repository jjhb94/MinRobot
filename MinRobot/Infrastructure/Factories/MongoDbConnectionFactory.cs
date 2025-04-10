using MongoDB.Driver;
using MinRobot.Api.Configuration;
using Microsoft.Extensions.Options;

namespace MinRobot.Infrastructure.Factories;

public class MongoDbConnectionFactory
{
    public IMongoClient Client { get; }
    public IMongoDatabase Database { get; }

    public MongoDbConnectionFactory(IOptions<MongoDbSettings> settings)
    {
        Client = new MongoClient(settings.Value.ConnectionString);
        Database = Client.GetDatabase(settings.Value.DatabaseName);
    }
}