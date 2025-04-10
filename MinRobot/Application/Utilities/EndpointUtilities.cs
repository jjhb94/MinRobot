
using System.Net;
using MinRobot.Application.Dto;
using MinRobot.Domain.Models;

namespace MinRobot.Application.Utilities;

public  class EndpointUtilities
{
    public static (RobotCommand, IResult) ValidateAndMapCommand(int? commandId, RobotCommandDto commandDto)
    {
        // Input Validation
        if (string.IsNullOrEmpty(commandDto.RobotId) || string.IsNullOrEmpty(commandDto.CommandData) || string.IsNullOrEmpty(commandDto.Status))
        {
            return (null!, Results.BadRequest(new RobotCommandResponse<string>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { "RobotId, CommandData, and Status are required." }
            }));
        }

        // validate CommandType enum format
        if (string.IsNullOrEmpty(commandDto.CommandType))
        {
            return (null!, Results.BadRequest(new RobotCommandResponse<string>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { "CommandType is required." }
            }));
        }

        // Extract the enum name from the string
        string commandTypeName = commandDto.CommandType.Split('(')[0]; // Get "Rotate"

        if (!Enum.IsDefined(typeof(CommandTypeEnum), commandTypeName))
        {
            return (null!, Results.BadRequest(new RobotCommandResponse<string>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { "Invalid CommandType." }
            }));
        }

        // Parse Status
        if (!Enum.TryParse(commandDto.Status, true, out CommandStatusEnum status))
        {
            return (null!, Results.BadRequest(new RobotCommandResponse<string>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { "Invalid status value." }
            }));
        }

        // special case for rotate command
        if (commandDto.CommandType.StartsWith("Rotate(", StringComparison.OrdinalIgnoreCase))
        {
            var degreeString = commandDto.CommandType.Substring(7, commandDto.CommandType.Length - 8);  // Extracting degrees from rotate(90)
            if (double.TryParse(degreeString, out double degrees))
            {
                commandDto.CommandType = "Rotate";  // Normalize command type to generic "Rotate"
                commandDto.CommandData = $"Degrees:{degrees}";  // Save degrees in CommandData
            }
            else
            {
                return (null!, Results.BadRequest(new RobotCommandResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "Invalid rotation degrees." }
                }));
            }
        }

        // Map RobotCommandDto to RobotCommand
        var command = new RobotCommand
        {
            Id = commandId.ToString(), // Use provided commandId or default(0)
            RobotId = commandDto.RobotId.ToUpper(),
            CommandType = commandDto.CommandType,
            CommandData = commandDto.CommandData,
            CreatedAt = DateTime.UtcNow,
            Status = status.ToString(),
        };

        return (command, null!); // Return command and null result (no error)
    }

    public static IResult ValidateCommandId(string commandId)
    {
        // Validate commandId is a positive integer
        if (!int.TryParse(commandId, out int parsedCommandId) || parsedCommandId <= 0)
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

    public static IResult HandleException(string message, Exception ex)
    {
        return Results.Problem(
            new RobotCommandResponse<string>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { message, ex.Message }
            }.ErrorMessages.FirstOrDefault(),
            statusCode: (int)HttpStatusCode.InternalServerError
        );
    }
}