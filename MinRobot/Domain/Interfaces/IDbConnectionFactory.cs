using System.Data;

namespace MinRobot.Domain.Interfaces;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellation = default);
}