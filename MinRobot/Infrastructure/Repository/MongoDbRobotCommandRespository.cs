namespace MinRobot.Infrastructure.Repository;

public class MongoDbRobotCommandRepository : IRobotCommandRepository
{
    private readonly IMongoCollection<RobotCommand> _robotCommandsCollection;

    public MongoDbRobotCommandRepository(IMongoDatabase database)
    {
        _robotCommandsCollection = database.GetCollection<RobotCommand>("robot_commands");
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
            // Handle exception (e.g., log it)
            Console.WriteLine($"Error inserting robot command: {ex.Message}");
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
            // Handle exception (e.g., log it)
            Console.WriteLine($"Error udapting robot command: {ex.Message}");
            throw new Exception("Error udpating robot command for MongoDB", ex);
        }
    }

    public async Task<RobotCommand?> GetRobotCommandByIdAsync(string commandId, CancellationToken cancellationToken)
    {
        try
        {
            var filter = Builders<RobotCommand>.Filter.Eq(c => c.Id, commandId);
            return await _robotCommandsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Handle exception (e.g., log it)
            Console.WriteLine($"Error getting robot command by Id: {ex.Message}");
            throw new Exception("Error getting robot command by from MongoDB", ex);
        }
    }
}
