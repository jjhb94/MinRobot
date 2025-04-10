// using Dapper;
// using MinRobot.Domain.Interfaces;
// using MinRobot.Domain.Models;

// namespace MinRobot.Infrastructure.Repository;

// public class PostgreSqlRobotStatusRepository : IRobotStatusRepository
// {
//     private readonly IGenericDbConnectionFactory _connectionFactory;

//     public PostgreSqlRobotStatusRepository(IGenericDbConnectionFactory connectionFactory)
//     {
//         _connectionFactory = connectionFactory;
//     }

//     public async Task<IEnumerable<RobotStatus>> GetAllStatusesAsync(CancellationToken cancellationToken)
//     {
//         using var db = await _connectionFactory.CreateConnectionAsync(cancellationToken);
//         return await db.QueryAsync<RobotStatus>(@"
//                 SELECT 
//                     robot_id AS RobotId, 
//                     status AS Status, 
//                     battery_level AS BatteryLevel, 
//                     uptime AS UptimeSeconds, 
//                     last_updated AS LastUpdated, 
//                     position_x AS PositionX, 
//                     position_y AS PositionY 
//                 FROM robot_statuses");
//     }

//     public async Task<RobotStatus?> GetRobotStatusByIdAsync(string robotId, CancellationToken cancellationToken)
//     {
//         using var db = await _connectionFactory.CreateConnectionAsync(cancellationToken);
//         return await db.QueryFirstOrDefaultAsync<RobotStatus>(@"
//                 SELECT 
//                     robot_id AS RobotId, 
//                     status AS Status, 
//                     battery_level AS BatteryLevel, 
//                     uptime AS uptimeSeconds, 
//                     last_updated AS LastUpdated, 
//                     position_x AS PositionX, 
//                     position_y AS PositionY 
//                 FROM robot_statuses 
//                 WHERE robot_id = @RobotId", new { RobotId = robotId });
//     }

//     public async Task AddRobotStatusAsync(RobotStatus status, CancellationToken cancellationToken)
//     {
//         using var db = await _connectionFactory.CreateConnectionAsync(cancellationToken);
//         await db.ExecuteAsync(
//             "INSERT INTO robot_statuses (robot_id, status, battery_level, uptime, last_updated, position_x, position_y) " +
//             "VALUES (@RobotId, @Status, @BatteryLevel, @UptimeSeconds, @LastUpdated, @PositionX, @PositionY)",
//             status);
//     }

//     public async Task UpdateRobotLastUpdatedAsync(string robotId, DateTime lastUpdated, CancellationToken cancellationToken)
//     {
//         using var db = await _connectionFactory.CreateConnectionAsync(cancellationToken);
//         await db.ExecuteAsync("UPDATE robot_statuses SET last_updated = @LastUpdated WHERE robot_id = @RobotId", new { RobotId = robotId, LastUpdated = lastUpdated });
//     }
// }