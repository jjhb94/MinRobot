using MinRobot.Models;

namespace MinRobot.Endpoints;

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

        group.MapGet("/", GetAllRobotStatuses);
        group.MapGet("/{robotId}", GetRobotStatusById);
    }

    private static IResult GetAllRobotStatuses()
    {
        return Results.Ok(_robotStatuses);
    }

    private static IResult GetRobotStatusById(string robotId)
    {
        var robot = _robotStatuses.FirstOrDefault(robot => robot.RobotId.Equals(robotId, StringComparison.OrdinalIgnoreCase));
        return robot is not null ? Results.Ok(robot) : Results.NotFound(new { Message = "Robot not found. :/ "});
    }

}