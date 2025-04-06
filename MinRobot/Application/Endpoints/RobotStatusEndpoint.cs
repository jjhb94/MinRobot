using MinRobot.Domain.Models;
using MinRobot.Application;

namespace MinRobot.Application.Endpoints;

public static class RobotStatusEndpoint
{
    // in-memory list of "robots"
    private static readonly List<RobotStatus> _robotStatuses = new()
    {
        new RobotStatus { RobotId = "TX-010", Status = "online", LastUpdated = DateTime.UtcNow },
        new RobotStatus { RobotId = "TX-023", Status = "offline", LastUpdated = DateTime.UtcNow }, 
        // TODO: add a battery status! need a better robot return type very strict! "RobotResponse" object

    };
    public static void MapRobotStatusEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/status");

        group.MapGet("/", async (DatabaseService db, CancellationToken cancellation) =>
        {
            var robots = await db.GetAllStatusesAsync(cancellation);
            return Results.Ok(robots);
        });

        // TODO: one thing I don't like is the robotId as string, this should be stricter in type or some kind of guid like property / key value property
        // for faster lookups and restrictions on what can be sent. 
        group.MapGet("/{robotId}", async (string robotId, DatabaseService db, CancellationToken cancellation) =>
        {
            var robot = await db.GetRobotStatusByIdAsync(robotId, cancellation);
            return Results.Ok(robot);
        });
    }

}