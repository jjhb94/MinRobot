using MinRobot.Domain.Models;
using MinRobot.Domain.Interfaces;
namespace MinRobot.Application;

public class DatabaseService
{
    private readonly IRobotStatusRepository _robotStatusRepository;
    private readonly IRobotCommandRepository _robotCommandRepository;

    public DatabaseService(IRobotStatusRepository robotStatusRepository, IRobotCommandRepository robotCommandRepository)
    {
        _robotCommandRepository = robotCommandRepository;
        _robotStatusRepository = robotStatusRepository;
    }

    public async Task<IEnumerable<RobotStatus>> GetAllStatusesAsync(CancellationToken cancellationToken)
    {
        return await _robotStatusRepository.GetAllStatusesAsync(cancellationToken);
    }

    // Fetch single robot by ID
    public async Task<RobotStatus?> GetRobotStatusByIdAsync(string robotId, CancellationToken cancellationToken)
    {
        return await _robotStatusRepository.GetRobotStatusByIdAsync(robotId, cancellationToken);
    }

    // Insert a robot status (for seeding or new robots)
    public async Task AddRobotStatusAsync(RobotStatus status, CancellationToken cancellationToken)
    {
        await _robotStatusRepository.AddRobotStatusAsync(status, cancellationToken);
    }
    
    // TODO: for larger applications Create Service folder and put break services up
    public async Task AddRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken)
    {
        await _robotCommandRepository.AddRobotCommandAsync(command, cancellationToken);
    }

    public async Task UpdateRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken)
    {
        await _robotCommandRepository.UpdateRobotCommandAsync(command, cancellationToken);
    }

    public async Task<RobotCommand?> GetRobotCommandByIdAsync(int commandId, CancellationToken cancellationToken)
    {
        return await _robotCommandRepository.GetRobotCommandByIdAsync(commandId, cancellationToken);
    }

}