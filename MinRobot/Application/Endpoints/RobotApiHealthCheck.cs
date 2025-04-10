using Microsoft.OpenApi.Models;

namespace MinRobot.Application.Endpoints;

public static class RobotApiHealthCheckEndpoint
{
    public static void MapHealthCheckEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            DatabaseStatus = "OK", // Example: Add database status
            Version = "1.0.0" // Example: Add API version
        }))
        .WithTags("Health")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "API Health Check",
            Description = "Checks the health and availability of the API.",
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "API is healthy.",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    ["Status"] = new OpenApiSchema { Type = "string", Description = "Health status of the API." },
                                    ["Timestamp"] = new OpenApiSchema { Type = "string", Format = "date-time", Description = "Timestamp of the health check." },
                                    ["DatabaseStatus"] = new OpenApiSchema { Type = "string", Description = "Database connection status." },
                                    ["Version"] = new OpenApiSchema { Type = "string", Description = "API Version" }
                                }
                            }
                        }
                    }
                },
                ["503"] = new OpenApiResponse
                {
                    Description = "API is unavailable.",
                }
            }
        });
    }
}