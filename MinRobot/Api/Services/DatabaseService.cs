namespace MinRobot.Api.Services;

public class DatabaseService
{
   private readonly IRobotStatusRepository _robotStatusRepository;
    private readonly IRobotCommandRepository _robotCommandRepository;
    private readonly IRobotHisotryRepository _robotHistoryRepository;
    private readonly ILogger<DatabaseService> _logger; // Add logger

    public DatabaseService(
        IRobotStatusRepository robotStatusRepository,
        IRobotCommandRepository robotCommandRepository,
        IRobotHisotryRepository robotHistoryRepository,
        ILogger<DatabaseService> logger) // Add logger
    {
        _robotStatusRepository = robotStatusRepository;
        _robotCommandRepository = robotCommandRepository;
        _robotHistoryRepository = robotHistoryRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<RobotStatus>> GetAllStatusesAsync(CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("Getting all statuses");
            return await _robotStatusRepository.GetAllStatusesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting all statuses: {ex}");
            throw;
        }
    }

    // Fetch single robot by ID
    public async Task<RobotStatus?> GetRobotStatusByIdAsync(string robotId, CancellationToken cancellationToken)
    {
        return await _robotStatusRepository.GetRobotStatusByIdAsync(robotId, cancellationToken);
    }

    // Insert a robot status (for seeding or new robots) // not implemented
    // public async Task AddRobotStatusAsync(RobotStatus status, CancellationToken cancellationToken)
    // {
    //     await _robotStatusRepository.AddRobotStatusAsync(status, cancellationToken);
    // }

    private async Task UpdateRobotLastUpdated(string robotId, string commandType, CancellationToken cancellationToken)
    {
        await _robotHistoryRepository.UpdateRobotStatusAndAddHistoryAsync(robotId, commandType, cancellationToken);
    }
    
    // TODO: for larger applications Create Service folder and put break services up
    public async Task<string> AddRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken)
    {
        var commandId = await _robotCommandRepository.AddRobotCommandAsync(command, cancellationToken);
        await UpdateRobotLastUpdated(command.RobotId, command.CommandType, cancellationToken); // Add this
        return commandId;
    }

    public async Task UpdateRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken)
    {
        await _robotCommandRepository.UpdateRobotCommandAsync(command, cancellationToken);
        await UpdateRobotLastUpdated(command.RobotId, command.CommandType, cancellationToken);
    }

    public async Task<RobotCommand?> GetRobotCommandByIdAsync(string commandId, CancellationToken cancellationToken)
    {
        return await _robotCommandRepository.GetRobotCommandByIdAsync(commandId, cancellationToken);
    }

    // history!
    public async Task<IEnumerable<RobotCommandHistoryDto>> GetRobotCommandHistoryAsync(string robotId, CancellationToken cancellationToken)
    {
        return await _robotHistoryRepository.GetRobotCommandHistoryAsync(robotId, cancellationToken);
    }

}