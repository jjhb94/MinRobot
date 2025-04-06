using MinRobot.Domain.Models;
using MinRobot.Domain.Interfaces;
namespace MinRobot.Application;

public class DatabaseService
{
    private readonly IRobotStatusRepository _robotStatusRepository;

    public DatabaseService(IRobotStatusRepository robotStatusRepository)
    {
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
}