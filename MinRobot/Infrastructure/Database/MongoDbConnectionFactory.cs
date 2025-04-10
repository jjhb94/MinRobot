using MinRobot.Api.Configuration;

namespace MinRobot.Infrastructure.Database;

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