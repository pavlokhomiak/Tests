using Npgsql;

namespace IntegrationTests.Tests;

public class DatabaseSetup
{
    public static async Task CreateDatabase(string connectionString, string db)
    {
        await using (NpgsqlConnection connection = new(connectionString))
        {
            await connection.OpenAsync();

            bool exists;
            var commandExists = @$"select exists(
                                    SELECT datname FROM pg_catalog.pg_database 
                                    WHERE lower(datname) = lower('{db}'));";
            await using (var c = new NpgsqlCommand(commandExists, connection))
            {
                exists = (bool) await c.ExecuteScalarAsync();
            }
            
            if (exists) return;

            var command = @$"create database {db}
                               with owner = postgres
                               encoding = 'UTF8'
                               connection limit = -1;";
            await using (var c = new NpgsqlCommand(command, connection))
                await c.ExecuteNonQueryAsync();
        }

        connectionString += $"Database={db}";
        await using (NpgsqlConnection connection = new(connectionString))
        {
            await connection.OpenAsync();
            var command = @"create table animals(
                                id serial PRIMARY KEY, 
                                name text, 
                                type text)";
            await using (var c = new NpgsqlCommand(command, connection))
                await c.ExecuteNonQueryAsync();
        }
    }

    public static async Task DeleteDatabase(string connectionString, string db)
    {
        NpgsqlConnection connection = new(connectionString);
        await connection.OpenAsync();

        string command = $@"SELECT pg_terminate_backend(pg_stat_activity.pid)
                               FROM pg_stat_activity
                               WHERE pg_stat_activity.datname = '{db}'";

        await using (var c = new NpgsqlCommand(command, connection))
            await c.ExecuteNonQueryAsync();

        await using (var c = new NpgsqlCommand($"drop database {db}", connection))
            await c.ExecuteNonQueryAsync();

        await connection.CloseAsync();
    }
}