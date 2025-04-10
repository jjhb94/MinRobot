using MinRobot.Infrastructure.Serialization;
using System;

namespace MinRobot.Infrastructure.Repository;

public class MongoDbRobotCommandRepository : IRobotCommandRepository
{
    private readonly IMongoCollection<RobotCommand> _robotCommandsCollection;
    private readonly ILogger<MongoDbRobotCommandRepository> _logger; // Add logger

    public MongoDbRobotCommandRepository(IMongoDatabase database, ILogger<MongoDbRobotCommandRepository> logger)
    {
        _robotCommandsCollection = database.GetCollection<RobotCommand>("robot_commands");
        _logger = logger; // Initialize logger
    }

    public async Task<string> AddRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _robotCommandsCollection.InsertOneAsync(command, new InsertOneOptions(), cancellationToken);
            return command.Id;
        }
        catch (Exception ex)
        {
            _logger?.LogError($"Error inserting robot command: {ex.Message}"); // Log the error
            throw new Exception("Error inserting robot command into MongoDB", ex);
        }
    }

    public async Task UpdateRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var filter = Builders<RobotCommand>.Filter.Eq(c => c.Id, command.Id);
            await _robotCommandsCollection.ReplaceOneAsync(filter, command, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"Error updating robot command: {ex.Message}"); // Log the error
            throw new Exception("Error udpating robot command for MongoDB", ex);
        }
    }

    public async Task<RobotCommand?> GetRobotCommandByIdAsync(string commandId, CancellationToken cancellationToken)
    {
        try
        {
            var objectId = GuidObjectIdConverter.GetMatchingObjectId(commandId);

            var filter = Builders<RobotCommand>.Filter.Eq(c => c.Id, objectId.ToString());
            return await _robotCommandsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Handle exception (e.g., log it)
            _logger?.LogError($"Error getting robot command by ID: {ex.Message}"); // Log the error
            throw new Exception("Error getting robot command by from MongoDB", ex);
        }
    }
}
