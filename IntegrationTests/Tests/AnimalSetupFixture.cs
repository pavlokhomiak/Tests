using IntegrationTests.Database;
using Npgsql;
using Xunit;

namespace IntegrationTests.Tests;

public class AnimalSetupFixture : IAsyncLifetime
{
    private const string ConnectionTemplate = "Server=localhost;User Id=postgres;Password=mysecretpassword;Port=54322;";
    private const string DatabaseName = "test_db";
    private const string ConnectionString = $"{ConnectionTemplate}Database={DatabaseName};";
    
    private PostgresqlConnectionFactory _connectionFactory;
    public IAnimalStore Store { get; private set; }

    public async Task InitializeAsync()
    {
        await DatabaseSetup.CreateDatabase(ConnectionTemplate, DatabaseName);

        _connectionFactory = new(ConnectionString);
        NpgsqlConnection connection = await _connectionFactory.Create();
        var database = new Postgresql(connection);
        Store = new AnimalStore(database);
        await Seed();
    }

    public async Task Seed()
    {
        await Store.SaveAnimal(new(0, "Foo", "Bar"));
        await Store.SaveAnimal(new(0, "Bar", "Bar"));
        await Store.SaveAnimal(new(0, "Baz", "Bar"));
    }

    public async Task DisposeAsync()
    {
        await _connectionFactory.DisposeAsync();
        await DatabaseSetup.DeleteDatabase(ConnectionTemplate, DatabaseName);
    }
}