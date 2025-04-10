using MinRobot.Domain.Interfaces;
using MinRobot.Infrastructure.Repository;
using MinRobot.Api.Configuration;
using MongoDB.Bson.Serialization;
using MinRobot.Infrastructure.Serialization;
using Microsoft.Extensions.Options;
using MinRobot.Api.Services;
using MongoDB.Driver;

namespace MinRobot.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        // Get connection string from appsettings.json
        // string? connectionString = configuration.GetConnectionString("PostgreSqlConnection");

        // if (string.IsNullOrEmpty(connectionString))
        // {
        //     throw new InvalidOperationException("Db connection is missing or empty!");
        // }

        // Register PostgreSqlDbConnectionFactory with the connection string
        // services.AddScoped<IGenericDbConnectionFactory>(provider =>
        //     new PostgreSqlDbConnectionFactory(connectionString));
        // Register RobotStatus, RobotCommand, and RobotHistory repositories
        // services.AddScoped<IRobotStatusRepository, PostgreSqlRobotStatusRepository>();
        // services.AddScoped<IRobotCommandRepository, PostgreSqlRobotCommandRepository>();
        // services.AddScoped<IRobotHisotryRepository, PostgreSqlRobotHistoryRepository>();
        // Register MongoDB client (singleton as recommended by MongoDB)
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        BsonSerializer.RegisterSerializer(new CommandTypeEnumConverter());
        services.AddSingleton<IMongoClient>(sp => 
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            logger.LogInformation($"MongoDB Connection String: {settings.ConnectionString}"); //add log here
            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(settings.ConnectionString));
            
            // Important security settings
            clientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);
            clientSettings.ConnectTimeout = TimeSpan.FromSeconds(5);
            clientSettings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            
            return new MongoClient(clientSettings);
        });

        // Register MongoDB database instance (scoped to match request lifetime)
        services.AddScoped<IMongoDatabase>(sp => 
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return client.GetDatabase(settings.DatabaseName);
        });
        // Register MongoDbConnectionFactory with the connection string    
        // services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        // services.AddSingleton<MongoDbConnectionFactory>();
        

        // regiser MongoRobotStatus, MongoRobotCommand, and MongoRobotHistory repositories
        services.AddScoped<IRobotStatusRepository, MongoDbRobotStatusRepository>();
        services.AddScoped<IRobotCommandRepository, MongoDbRobotCommandRepository>();
        services.AddScoped<IRobotHisotryRepository, MongoDbRobotHistoryRepository>();
        // Register DatabaseService
        services.AddScoped<DatabaseService>();

        // Add other application service registrations here...

        return services;
    }
}