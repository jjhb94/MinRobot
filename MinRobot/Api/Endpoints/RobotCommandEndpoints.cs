namespace MinRobot.Api.Endpoints;

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
            if (!EndpointUtilities.IsValidRobotId(commandDto.RobotId.ToUpper()))
            {
                return Results.BadRequest(new RobotCommandResponse<RobotCommand>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "Invalid RobotId format. Expected format: 'TX-123'." }
                });
            }

            var (command, errorResult) = EndpointUtilities.ValidateAndMapCommand(null, commandDto);
            if (errorResult != null) return errorResult;
            //RobotStatus commandData = ParseCommandDataToPosition.ProcessCommandAndUpdateStatus(command, command.Status);
            // TODO: update the response to store x y coordinates from the RobotCommand data object to RobotStatus (robotId, robot.x, robot.y)

            var commandId = await db.AddRobotCommandAsync(command, cancellation);
            command.Id = commandId.ToString();

            return Results.Created($"/api/command/{command.Id}", new RobotCommandResponse<RobotCommand>
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

    private static async Task<IResult> PutRobotCommandAsync(string commandId, RobotCommandDto commandDto, DatabaseService db, CancellationToken cancellation)
    {
        try
        {
            if (!EndpointUtilities.IsValidRobotId(commandDto.RobotId.ToUpper()))
            {
                return Results.BadRequest(new RobotCommandResponse<RobotCommand>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "Invalid RobotId format. Expected format: 'TX-123'." }
                });
            }

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

    private static async Task<IResult> GetRobotCommandAsync(string commandId, DatabaseService db, CancellationToken cancellation)
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