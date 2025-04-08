using Dapper;
using MinRobot.Application.Dto;
using MinRobot.Domain.Interfaces;
using MinRobot.Domain.Models;

namespace MinRobot.Infrastructure.Repository;

public class PostgreSqlRobotHistoryRepository : IRobotHisotryRepository
{
    private readonly IGenericDbConnectionFactory _connectionFactory;

    public PostgreSqlRobotHistoryRepository(IGenericDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task<IEnumerable<RobotCommandHistoryDto>> GetRobotCommandHistoryAsync(string robotId, CancellationToken cancellationToken)
    {
        using var db = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        return await db.QueryAsync<RobotCommandHistoryDto>(@"
        SELECT command_id AS CommandId, command_type AS CommandType, command_data AS CommandData
        FROM robot_commands
        WHERE robot_id = @RobotId", new { RobotId = robotId });
    }
}