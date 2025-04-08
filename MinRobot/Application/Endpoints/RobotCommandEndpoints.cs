// MinRobot.Application/Endpoints/RobotCommandEndpoints.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MinRobot.Application.Dto;
using MinRobot.Domain.Models;
using System.Net;

namespace MinRobot.Application.Endpoints;

public static class RobotCommandEndpoints
{
    public static void MapRobotCommandEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/command");

        group.MapPost("/", PostRobotCommandAsync);
        group.MapPut("/{commandId}", PutRobotCommandAsync);
        group.MapGet("/{commandId}", GetRobotCommandAsync);
    }

    private static async Task<IResult> PostRobotCommandAsync(RobotCommandDto commandDto, DatabaseService db, CancellationToken cancellation)
    {
        try
        {
            // Map RobotCommandDto to RobotCommand (if necessary)
            var command = new RobotCommand
            {
                RobotId = commandDto.RobotId.ToUpper(),
                CommandType = commandDto.CommandType,
                CommandData = commandDto.CommandData,
                CreatedAt = DateTime.UtcNow,
                Status = "pending", // TODO: commandStatusEnum maybe use that here and udpate the RobotCommand model
                //  // Optional, set to null initially
            };
            // Save the command to the database
            await db.AddRobotCommandAsync(command, cancellation);

            return Results.Created($"/api/command/{command.CommandId}", new RobotCommandResponse<RobotCommand>
            {
                Data = command,
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created
            });
        }
        catch (Exception ex)
        {
            return HandleException("An error occurred while adding the command.", ex);
        }
    }

    private static async Task<IResult> PutRobotCommandAsync(int commandId, RobotCommandDto commandDto, DatabaseService db, CancellationToken cancellation)
    {
        try
        {
            // Map RobotCommandDto to RobotCommand (if necessary)
            var command = new RobotCommand
            {
                CommandId = commandId,
                RobotId = commandDto.RobotId,
                CommandType = commandDto.CommandType,
                CommandData = commandDto.CommandData,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending" // TODO: commandStatusEnum maybe use that here and udpate the RobotCommand model
            };

            // Update the command in the database
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
            return HandleException($"An error occurred while updating command ID {commandId}.", ex);
        }
    }

    private static async Task<IResult> GetRobotCommandAsync(int commandId, DatabaseService db, CancellationToken cancellation)
    {
        try
        {
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
            return HandleException($"An error occurred while retrieving command ID {commandId}.", ex);
        }
    }

    private static IResult HandleException(string message, Exception ex)
    {
        return Results.Problem(new RobotCommandResponse<string>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.InternalServerError,
            ErrorMessages = new List<string> { message, ex.ToString() }
        }.ErrorMessages.FirstOrDefault(), statusCode: (int)HttpStatusCode.InternalServerError);
    }
}