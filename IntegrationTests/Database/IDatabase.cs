using Npgsql;

namespace IntegrationTests.Database;

public interface IDatabase
{
    Task<NpgsqlDataReader> Query(string query, Dictionary<string, object> parameters = null);
    Task Insert(string command, Dictionary<string, string> parameters = null);
}