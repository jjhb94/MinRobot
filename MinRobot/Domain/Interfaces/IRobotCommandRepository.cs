namespace MinRobot.Domain.Interfaces;
public interface IRobotCommandRepository {
    Task<string> AddRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken);
    Task UpdateRobotCommandAsync(RobotCommand command, CancellationToken cancellationToken);
    Task<RobotCommand?> GetRobotCommandByIdAsync(string commandId, CancellationToken cancellationToken);
}