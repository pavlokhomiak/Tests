using IntegrationTests.Database;
using Npgsql;
using Xunit;

namespace IntegrationTests.Tests;

public class AnimalDatabaseTests
{
    private const string ConnectionTemplate = "Server=localhost;User Id=postgres;Password=mysecretpassword;Port=54322;";
    private const string DatabaseName = "test_db";
    private const string ConnectionString = $"{ConnectionTemplate}Database={DatabaseName};";

    [Fact]
    public async void AnimalStore_SavesAnimalToDatabase()
    {
        await DatabaseSetup.CreateDatabase(ConnectionTemplate, DatabaseName);

        PostgresqlConnectionFactory connectionFactory = new PostgresqlConnectionFactory(ConnectionString);
        NpgsqlConnection connection = await connectionFactory.Create();
        IDatabase database = new Postgresql(connection);
        IAnimalStore store = new AnimalStore(database);

        await store.SaveAnimal(new(0, "Foo", "Bar"));

        var animals = await store.GetAnimals();
        var animal = Assert.Single(animals);
        Assert.Equal(1, animal.Id);
        Assert.Equal("Foo", animal.Name);
        Assert.Equal("Bar", animal.Type);

        await connectionFactory.DisposeAsync();
        await DatabaseSetup.DeleteDatabase(ConnectionTemplate, DatabaseName);
        NpgsqlConnection.ClearAllPools();
    }
    
    [Fact]
    public async void AnimalStore_GetsSavedAnimalByIdFromDatabase()
    {
        await DatabaseSetup.CreateDatabase(ConnectionTemplate, DatabaseName);

        PostgresqlConnectionFactory connectionFactory = new PostgresqlConnectionFactory(ConnectionString);
        NpgsqlConnection connection = await connectionFactory.Create();
        IDatabase database = new Postgresql(connection);
        IAnimalStore store = new AnimalStore(database);

        await store.SaveAnimal(new(0, "Foo", "Bar"));

        var animal = await store.GetAnimal(1);

        Assert.NotNull(animal);
        Assert.Equal(1, animal.Id);
        Assert.Equal("Foo", animal.Name);
        Assert.Equal("Bar", animal.Type);

        await connectionFactory.DisposeAsync();
        await DatabaseSetup.DeleteDatabase(ConnectionTemplate, DatabaseName);
        NpgsqlConnection.ClearAllPools();
    }
}