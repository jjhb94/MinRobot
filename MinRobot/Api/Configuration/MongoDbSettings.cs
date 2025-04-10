namespace MinRobot.Api.Configuration
{
    public class MongoDbSettings
    {
        public required string ConnectionString { get; set; } // could also use the Required attribute
        public required string DatabaseName { get; set; } // here too

    }    
}