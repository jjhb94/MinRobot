
using System.Net;
using MinRobot.Application.Dto;
using MinRobot.Domain.Models;

namespace MinRobot.Api.Utilities;

public static class EndpointUtilities
{
    public static IResult HandleException(string message, Exception ex)
    {
        return Results.Problem(new RobotCommandResponse<string>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.InternalServerError,
            ErrorMessages = new List<string> { message, ex.ToString() }
        }.ErrorMessages.FirstOrDefault(), statusCode: (int)HttpStatusCode.InternalServerError);
    }
    public static (RobotCommand, IResult) ValidateAndMapCommand(int? commandId, RobotCommandDto commandDto)
    {
        // Input Validation
        if (string.IsNullOrEmpty(commandDto.RobotId) || string.IsNullOrEmpty(commandDto.CommandType) || string.IsNullOrEmpty(commandDto.CommandData) || string.IsNullOrEmpty(commandDto.Status))
        {
            return (null!, Results.BadRequest(new RobotCommandResponse<string>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new System.Collections.Generic.List<string> { "RobotId, CommandType, CommandData, and Status are required." }
            }));
        }

        // Parse Status
        if (!Enum.TryParse(commandDto.Status, true, out CommandStatusEnum status))
        {
            return (null!, Results.BadRequest(new RobotCommandResponse<string>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new System.Collections.Generic.List<string> { "Invalid status value." }
            }));
        }

        // Map RobotCommandDto to RobotCommand
        var command = new RobotCommand
        {
            CommandId = commandId.GetValueOrDefault(), // Use provided commandId or default(0)
            RobotId = commandDto.RobotId.ToUpper(),
            CommandType = commandDto.CommandType,
            CommandData = commandDto.CommandData,
            CreatedAt = DateTime.UtcNow,
            Status = status.ToString(),
        };

        return (command, null!); // Return command and null result (no error)
    }

    public static IResult ValidateCommandId(int commandId)
    {
        if (commandId <= 0)
        {
            return Results.BadRequest(new RobotCommandResponse<string>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { "Invalid commandId. Must be a positive integer." }
            });
        }
        return null!;
    }
    public static bool IsValidRobotId(string robotId)
    {
        // Ensure RobotId matches the format "TX-123"
        return !string.IsNullOrEmpty(robotId) && System.Text.RegularExpressions.Regex.IsMatch(robotId, @"^[A-Z]{2}-\d+$");
    }
}