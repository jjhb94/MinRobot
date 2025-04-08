// PostgreSqlRobotCommandRepository.cs
using Dapper;
using MinRobot.Domain.Interfaces;
using MinRobot.Domain.Models;

namespace MinRobot.Infrastructure.Repository;

public class PostgreSqlRobotCommandRepository : IRobotCommandRepository
{
    private readonly IGenericDbConnectionFactory _connectionFactory;

    public PostgreSqlRobotCommandRepository(IGenericDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> AddRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken)
    {
        using var db = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        return await db.ExecuteScalarAsync<int>(@"
            INSERT INTO robot_commands (robot_id, command_type, command_data, created_at, status)
            VALUES (@RobotId, @CommandType, @CommandData, @CreatedAt, @Status)
            RETURNING command_id", new
        {
            command.RobotId,
            command.CommandType,
            command.CommandData,
            command.CreatedAt,
            command.Status
        });
    }

    public async Task UpdateRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken)
    {
        using var db = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        await db.ExecuteAsync(@"
            UPDATE robot_commands
            SET robot_id = @RobotId, command_type = @CommandType, command_data = @CommandData,
                created_at = @CreatedAt, status = @Status, error_message = @ErrorMessage
            WHERE command_id = @CommandId", command);
    }

    public async Task<RobotCommand?> GetRobotCommandByIdAsync(int commandId, CancellationToken cancellationToken)
    {
        using var db = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        return await db.QueryFirstOrDefaultAsync<RobotCommand>(@"
            SELECT command_id AS CommandId, robot_id AS RobotId, command_type AS CommandType, command_data AS CommandData,
                   created_at AS CreatedAt, status AS Status, error_message AS ErrorMessage
            FROM robot_commands
            WHERE command_id = @CommandId", new { CommandId = commandId });
    }
}