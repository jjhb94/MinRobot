using System.Data;
using MinRobot.Domain.Interfaces;
using Npgsql;

namespace MinRobot.Infrastructure.Factories;

public class PostgreSqlDbConnectionFactory : IGenericDbConnectionFactory
{
    private readonly string _connectionString;

    public PostgreSqlDbConnectionFactory(string connectionString)
    {
            _connectionString = connectionString;
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
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }
}