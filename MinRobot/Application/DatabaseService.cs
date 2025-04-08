using MinRobot.Domain.Models;
using MinRobot.Domain.Interfaces;
using MinRobot.Application.Dto;
namespace MinRobot.Application;

public class DatabaseService
{
    private readonly IRobotStatusRepository _robotStatusRepository;
    private readonly IRobotCommandRepository _robotCommandRepository;
    private readonly IRobotHisotryRepository _robotHistoryRepository;

    public DatabaseService(IRobotStatusRepository robotStatusRepository, IRobotCommandRepository robotCommandRepository, IRobotHisotryRepository robotHistoryRepository)
    {
        _robotCommandRepository = robotCommandRepository;
        _robotStatusRepository = robotStatusRepository;
        _robotHistoryRepository = robotHistoryRepository;
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
    public async Task<int> AddRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken)
    {
        return await _robotCommandRepository.AddRobotCommandAsync(command, cancellationToken);
    }

    public async Task UpdateRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken)
    {
        await _robotCommandRepository.UpdateRobotCommandAsync(command, cancellationToken);
    }

    public async Task<RobotCommand?> GetRobotCommandByIdAsync(int commandId, CancellationToken cancellationToken)
    {
        return await _robotCommandRepository.GetRobotCommandByIdAsync(commandId, cancellationToken);
    }

    // history!
    public async Task<IEnumerable<RobotCommandHistoryDto>> GetRobotCommandHistoryAsync(string robotId, CancellationToken cancellationToken)
    {
        return await _robotHistoryRepository.GetRobotCommandHistoryAsync(robotId, cancellationToken);
    }

}