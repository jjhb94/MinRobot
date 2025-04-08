using System.Net;
using MinRobot.Domain.Models;
using MinRobot.Application.Dto;

namespace MinRobot.Application.Endpoints;

public static class RobotStatusEndpoint
{
    // in-memory list of "robots"
    // private static readonly List<RobotStatus> _robotStatuses = new()
    // {
    //     new RobotStatus { RobotId = "TX-010", Status = "online", LastUpdated = DateTime.UtcNow },
    //     new RobotStatus { RobotId = "TX-023", Status = "offline", LastUpdated = DateTime.UtcNow }, 
    //     // TODO: add a battery status! need a better robot return type very strict! "RobotResponse" object

    // };
    public static void MapRobotStatusEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/status");

        group.MapGet("/", GetAllRobotStatusesAsync);
        group.MapGet("/{robotId}", GetRobotStatusByIdAsync);

        // group.MapGet("/", async (DatabaseService db, CancellationToken cancellation) =>
        // {
        //     var robots = await db.GetAllStatusesAsync(cancellation);
        //     return Results.Ok(robots);
        // });

        // // TODO: one thing I don't like is the robotId as string, this should be stricter in type or some kind of guid like property / key value property
        // // for faster lookups and restrictions on what can be sent. 
        // group.MapGet("/{robotId}", async (string robotId, DatabaseService db, CancellationToken cancellation) =>
        // {
        //     var robot = await db.GetRobotStatusByIdAsync(robotId, cancellation);
        //     return Results.Ok(robot);
        // });
    }

    public static async Task<IResult> GetAllRobotStatusesAsync(DatabaseService db, CancellationToken cancellation)
    {
        try
        {
            var robots = await db.GetAllStatusesAsync(cancellation);
            if (!robots.Any())
            {
                return Results.NotFound(new RobotStatusResponse<List<RobotStatus>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { "No robot statuses found." }
                });
            }

            return Results.Ok(new RobotStatusResponse<List<RobotStatus>>
            {
                Data = robots.ToList(),
                ItemCount = robots.Count(),
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            });
        }
        catch (Exception ex)
        {
            return HandleException("An error occurred while retrieving robot statuses.", ex);
        }
    }

    public static async Task<IResult> GetRobotStatusByIdAsync(string robotId, DatabaseService db, CancellationToken cancellation)
    {
        try
        {
            // TODO: maybe use var robotLower = robotId.ToLower(); and then pass as parameter to ignore case sensitivity
            var robot = await db.GetRobotStatusByIdAsync(robotId.ToUpper(), cancellation);
            if (robot == null)
            {
                return Results.NotFound(new RobotStatusResponse<RobotStatus>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"Robot with ID {robotId} not found." }
                });
            }

            return Results.Ok(new RobotStatusResponse<RobotStatus>
            {
                Data = robot,
                ItemCount = 1,
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            });
        }
        catch (Exception ex)
        {
            return HandleException($"An error occurred while retrieving the status for robot ID {robotId}.", ex);
        }
    }

    public static IResult HandleException(string message, Exception ex)
    {
        return Results.Problem(new RobotStatusResponse<string>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.InternalServerError,
            ErrorMessages = new List<string> { message, ex.Message }
        }.ErrorMessages.FirstOrDefault(), statusCode: (int)HttpStatusCode.InternalServerError);
    }

}