using Npgsql;

namespace MinRobot.Infrastructure.Database;

public class PostgreSqlDbConnectionFactory : IGenericDbConnectionFactory
{
    private readonly string _connectionString;
    private readonly ILogger<PostgreSqlDbConnectionFactory> _logger;

    public PostgreSqlDbConnectionFactory(string connectionString, ILogger<PostgreSqlDbConnectionFactory> logger)
    {
            _connectionString = connectionString;
            _logger = logger;
    }
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentNullException(nameof(_connectionString));
            }

            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return connection;

        }
        catch (Exception ex)
        {
            _logger?.LogError($"An error occurred with creating database connection: {ex.Message}");
            throw new Exception("Failed to create a database connection.", ex);
        }
    }
}