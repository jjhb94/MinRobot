using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using MinRobot.Api.Dto;
using MinRobot.Domain.Models;
using MinRobot.Domain.Interfaces;

namespace MinRobot.Infrastructure.Repository
{
    public class MongoDbRobotHistoryRepository : IRobotHisotryRepository
    {
        private readonly IMongoCollection<RobotHistory> _robotHistoryCollection;
        private readonly IMongoCollection<RobotStatus> _robotStatusCollection;
        private readonly ILogger<MongoDbRobotHistoryRepository> _logger; // Add logger

        public MongoDbRobotHistoryRepository(IMongoDatabase database, ILogger<MongoDbRobotHistoryRepository> logger)
        {
            _logger = logger; // Initialize logger
            // Initialize the MongoDB collection for robot history
            _robotHistoryCollection = database.GetCollection<RobotHistory>("robot_history");
            _robotStatusCollection = database.GetCollection<RobotStatus>("robot_statuses");
            _logger = logger;

        }

        public async Task<IEnumerable<RobotCommandHistoryDto>> GetRobotCommandHistoryAsync(string robotId, CancellationToken cancellationToken)
        {
            try
            {
                // Case-insensitive search using Regex
                var filter = Builders<RobotHistory>.Filter.Regex(h => h.RobotId, new BsonRegularExpression(Regex.Escape(robotId), "i"));

                var history = await _robotHistoryCollection.Find(filter).ToListAsync(cancellationToken);

                _logger.LogInformation($"Retrieved {history.Count} raw history entries for robotId: {robotId}");

                foreach(var item in history)
                {
                    _logger.LogInformation($"Raw history item: {System.Text.Json.JsonSerializer.Serialize(item)}");
                }

                var projectedHistory = history
                    .Select(h => new RobotCommandHistoryDto
                    {
                        CommandId = h.Id.ToString(),
                        CommandType = h.CommandType.ToString(),
                        CommandData = h.CommandData
                    })
                    .ToList();

                _logger.LogInformation($"Retrieved {projectedHistory.Count} history entries for robotId: {robotId}");
                return projectedHistory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving robot command history for robotId: {robotId}");
                throw;
            }
        }
        public async Task UpdateRobotStatusAndAddHistoryAsync(string robotId, string commandType, CancellationToken cancellationToken)
        {
            try
            {
                // Update Robot Status
                try
                {
                    var filter = Builders<RobotStatus>.Filter.Eq(r => r.RobotId, robotId);
                    var update = Builders<RobotStatus>.Update.Set(r => r.LastUpdated, DateTime.UtcNow);
                    await _robotStatusCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
                    _logger.LogInformation($"Updated robot status for robotId: {robotId}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating robot status for robotId: {robotId}");
                    //Handle the error, or rethrow.
                    throw;
                }

                // Add History Record
                try
                {
                    CommandTypeEnum commandTypeEnum = (CommandTypeEnum)Enum.Parse(typeof(CommandTypeEnum), commandType, true);
                    await _robotHistoryCollection.InsertOneAsync(new RobotHistory
                    {
                        RobotId = robotId,
                        CommandType = commandTypeEnum,
                        CommandData = $"Last updated: {DateTime.UtcNow}",
                        Timestamp = DateTime.UtcNow
                    }, new InsertOneOptions(), cancellationToken);
                    _logger.LogInformation($"Added robot history for robotId: {robotId}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error adding robot history for robotId: {robotId}");
                    //Handle the error, or rethrow.
                    throw;
                }

                _logger.LogInformation($"Updated robot status and history for robotId: {robotId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating robot status and history for robotId: {robotId}");
                throw;
            }
        }
    }
}