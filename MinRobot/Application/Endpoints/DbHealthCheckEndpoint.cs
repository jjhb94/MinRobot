using MinRobot.Application.Dto;
using MinRobot.Application.Utilities;
using System.Net;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MinRobot.Application.Configuration;
using Microsoft.Extensions.Options;

namespace MinRobot.Application.Endpoints;

public static class DbHealthEndpoint
{
    public static void MapDbHealthEndpoint(this IEndpointRouteBuilder app)
    {        
        app.MapGet("/api/health/db", CheckDbHealthAsync);
    }

    public static async Task<IResult> CheckDbHealthAsync(
        [FromServices] IMongoClient mongoClient, // Explicit service injection
        [FromServices] IOptions<MongoDbSettings> mongoSettings,
        CancellationToken cancellation)
    {
        try
        {
            // Ping the database
            var pingCommand = new BsonDocumentCommand<BsonDocument>(
                new BsonDocument { { "ping", 1 } });
            
            await mongoClient.GetDatabase("minrobot_db")
                .RunCommandAsync(pingCommand, cancellationToken: cancellation);

            return Results.Ok("MongoDb conenction healthy");
        }
        catch (Exception ex)
        {
            return Results.Problem($"MongoDB connection failed: {ex.Message}");
        }
    }
}