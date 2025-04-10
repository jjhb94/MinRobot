// MinRobot.Application/Configuration/MongoDbSettings.cs
namespace MinRobot.Application.Configuration
{
    public class MongoDbSettings
    {
        public required string ConnectionString { get; set; } // could also use the Required attribute
        public required string DatabaseName { get; set; } // here too
    
        // public MongoDbSettings(string connectionString, string databaseName)
        // {
        //     ConnectionString = connectionString;
        //     DatabaseName = databaseName;
        // }
    }
    
}