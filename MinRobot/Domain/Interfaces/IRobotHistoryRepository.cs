namespace MinRobot.Domain.Interfaces;

public interface IRobotHisotryRepository
{
    Task<IEnumerable<RobotCommandHistoryDto>> GetRobotCommandHistoryAsync(string robotId, CancellationToken cancellationToken);
    Task UpdateRobotStatusAndAddHistoryAsync(string robotId, string commandType, CancellationToken cancellationToken);
}