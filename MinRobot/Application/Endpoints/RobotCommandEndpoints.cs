using MinRobot.Application.Dto;
using MinRobot.Api.Utilities; 
using System.Net;

namespace MinRobot.Application.Endpoints;

public static class RobotCommandEndpoints
{
    public static void MapRobotCommandEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/command");

        group.MapPost("/", PostRobotCommandAsync); // send a movement or command // i.e. one of the Robot
        group.MapPut("/{commandId}", PutRobotCommandAsync);
        group.MapGet("/{commandId}", GetRobotCommandAsync);
    }

    private static async Task<IResult> PostRobotCommandAsync(RobotCommandDto commandDto, DatabaseService db, CancellationToken cancellation)
    {
        try
        {
            var (command, errorResult) = EndpointUtilities.ValidateAndMapCommand(null, commandDto);
            if (errorResult != null) return errorResult;

            // handle rotation commands
            if (command.CommandType.ToLower().StartsWith("rotate("))
            {
                // Extract the degree value
                string degreesString = command.CommandType.Substring(7, command.CommandType.Length - 8); // Remove "Rotate(" and ")"
                if (double.TryParse(degreesString, out double degrees))
                {
                    // Store the degrees in the command data or a new field
                    command.CommandData = $"Degrees:{degrees}"; // Store in command data for example
                    command.CommandType = "Rotate"; // Store generic rotate command.
                }
                else
                {
                    return Results.BadRequest(new RobotCommandResponse<RobotCommand>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = new List<string> { "Invalid rotation degrees." }
                    });
                }
            }


            var commandId = await db.AddRobotCommandAsync(command, cancellation);
            command.CommandId = commandId;

            return Results.Created($"/api/command/{command.CommandId}", new RobotCommandResponse<RobotCommand>
            {
                Data = command,
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created
            });
        }
        catch (Exception ex)
        {
            return EndpointUtilities.HandleException("An error occurred while adding the command. Does the command match expected?", ex);
        }
    }

    private static async Task<IResult> PutRobotCommandAsync(int commandId, RobotCommandDto commandDto, DatabaseService db, CancellationToken cancellation)
    {
        try
        {
            var (command, errorResult) = EndpointUtilities.ValidateAndMapCommand(commandId, commandDto);
            if (errorResult != null) return errorResult;

            await db.UpdateRobotCommandAsync(command, cancellation);

            return Results.Ok(new RobotCommandResponse<RobotCommand>
            {
                Data = command,
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            });
        }
        catch (Exception ex)
        {
            return EndpointUtilities.HandleException($"An error occurred while updating command ID {commandId}.", ex);
        }
    }

    private static async Task<IResult> GetRobotCommandAsync(int commandId, DatabaseService db, CancellationToken cancellation)
    {
        try
        {
            var validationResult = EndpointUtilities.ValidateCommandId(commandId);
            if (validationResult != null) return validationResult;

            var command = await db.GetRobotCommandByIdAsync(commandId, cancellation);

            if (command == null)
            {
                return Results.NotFound(new RobotCommandResponse<RobotCommand>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"Command with ID {commandId} not found." }
                });
            }

            return Results.Ok(new RobotCommandResponse<RobotCommand>
            {
                Data = command,
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            });
        }
        catch (Exception ex)
        {
            return EndpointUtilities.HandleException($"An error occurred while retrieving command ID {commandId}.", ex);
        }
    }
}