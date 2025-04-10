namespace MinRobot.Domain.Interfaces
{
    public interface IRobotStatusRepository
    {
        Task<IEnumerable<RobotStatus>> GetAllStatusesAsync(CancellationToken cancellationToken);
        Task<RobotStatus?> GetRobotStatusByIdAsync(string robotId, CancellationToken cancellationToken);
        Task AddRobotStatusAsync(RobotStatus status, CancellationToken cancellationToken);
        // Task UpdateRobotLastUpdatedAsync(string robotId, DateTime lastUpdated, CancellationToken cancellationToken);
    }
}