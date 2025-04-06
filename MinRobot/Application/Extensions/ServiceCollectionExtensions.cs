// In MinRobot.Application/Extensions/ServiceCollectionExtensions.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinRobot.Application;
using MinRobot.Infrastructure.Factories;
using MinRobot.Domain.Interfaces;
using MinRobot.Infrastructure.Repository;

namespace MinRobot.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Get connection string from appsettings.json
        string? connectionString = configuration.GetConnectionString("PostgresConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Db connection is missing or empty!");
        }

        // Register PostgreSqlDbConnectionFactory with the connection string
        services.AddScoped<IGenericDbConnectionFactory>(provider => new PostgreSqlDbConnectionFactory(connectionString));

        // Register RobotStatusRepository
        services.AddScoped<IRobotStatusRepository, PostgreSqlRobotStatusRepository>();

        // Register DatabaseService
        services.AddScoped<DatabaseService>();

        // Add other application service registrations here...

        return services;
    }
}