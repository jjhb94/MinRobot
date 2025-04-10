namespace MinRobot.Infrastructure.Repository;

public class MongoDbRobotStatusRepository : IRobotStatusRepository
{
    private readonly IMongoCollection<RobotStatus> _robotStatusesCollection;

    public MongoDbRobotStatusRepository(IMongoDatabase database)
    {
        // Initialize the MongoDB collection for robot statuses
        _robotStatusesCollection = database.GetCollection<RobotStatus>("robot_statuses");
    }

    public async Task<IEnumerable<RobotStatus>> GetAllStatusesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _robotStatusesCollection.Find(Builders<RobotStatus>.Filter.Empty)
                .Project(Builders<RobotStatus>.Projection.As<RobotStatus>()) // Explicit projection
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting all statuses: {ex}");
            throw;
        }
    }

    public async Task<RobotStatus?> GetRobotStatusByIdAsync(string robotId, CancellationToken cancellationToken)
    {
        return await _robotStatusesCollection.Find(robot => robot.RobotId == robotId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddRobotStatusAsync(RobotStatus status, CancellationToken cancellationToken)
    {
        await _robotStatusesCollection.InsertOneAsync(status, new InsertOneOptions(), cancellationToken);
    }
}
