using Npgsql;

namespace IntegrationTests.Database;

public class PostgresqlConnectionFactory : IAsyncDisposable
{
    private NpgsqlConnection _connection;
    private readonly string _connectionString;

    public PostgresqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<NpgsqlConnection> Create()
    {
        // not async safe
        if (_connection != null)
            return _connection;

        _connection = new(_connectionString);
        await _connection.OpenAsync();
        return _connection;
    }

    public ValueTask DisposeAsync()
    {
        return _connection.DisposeAsync();
    }
}