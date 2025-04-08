using MinRobot.Domain.Models;
// IRobotCommandRepository.cs
public interface IRobotCommandRepository {
    Task<int> AddRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken);
    Task UpdateRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken);
    Task<RobotCommand?> GetRobotCommandByIdAsync(int commandId, CancellationToken cancellationToken);
}