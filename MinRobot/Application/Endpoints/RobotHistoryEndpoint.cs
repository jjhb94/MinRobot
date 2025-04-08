using MinRobot.Application.Dto;
using MinRobot.Api.Utilities;
using System.Net;
using MinRobot.Domain.Models;

namespace MinRobot.Application.Endpoints;

public static class RobotHistoryEndpoint
{
    public static void MapRobotHistoryEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/history");

        group.MapGet("/{robotId}", GetRobotHistoryAsync);
    }

    public static async Task<IResult> GetRobotHistoryAsync(string robotId, DatabaseService db, CancellationToken cancellation)
    {
        try
        {
            var history = await db.GetRobotCommandHistoryAsync(robotId.ToUpper(), cancellation);
            if (history == null || !history.Any()) // Check if history is null or empty
            {
                return Results.Ok(new RobotCommandHistoryResponse<IEnumerable<RobotCommandHistoryDto>>
                {
                    Data = null, // Return null if no history found
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }
            var dtoHistory = history.Select(h => new RobotCommandHistoryDto
            {
                CommandId = h.CommandId,
                CommandType = h.CommandType,
                CommandData = h.CommandData
            });

            return Results.Ok(new RobotCommandHistoryResponse<IEnumerable<RobotCommandHistoryDto>>
            {
                Data = dtoHistory,
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            });
        }
        catch (Exception ex)
        {
            return EndpointUtilities.HandleException($"An error occurred while retrieving history for robot {robotId}.", ex);
        }
    }
}