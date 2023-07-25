using IntegrationTests.Database;
using Npgsql;
using Xunit;

namespace IntegrationTests.Tests;

public class AnimalDatabaseTests : IClassFixture<AnimalSetupFixture>
{
    private readonly AnimalSetupFixture _animalSetupFixture;

    public AnimalDatabaseTests(AnimalSetupFixture animalSetupFixture)
    {
        _animalSetupFixture = animalSetupFixture;
    }

    [Fact]
    public async void AnimalStore_SavesAnimalToDatabase()
    {
        IAnimalStore store = _animalSetupFixture.Store;
        Animal animal = new(0, "Pavlo", "Bar");
        await store.SaveAnimal(animal);

        var animals = await store.GetAnimals();
        var storedAnimal= animals.Where(a => a.Name == "Pavlo").First();

        Assert.Equal(4, storedAnimal.Id);
        Assert.Equal("Pavlo", storedAnimal.Name);
        Assert.Equal("Bar", storedAnimal.Type);
        
        // NpgsqlConnection.ClearAllPools();
    }
    
    [Fact]
    public async void AnimalStore_GetsSavedAnimalByIdFromDatabase()
    {
        IAnimalStore store = _animalSetupFixture.Store;

        var animal = await store.GetAnimal(1);

        Assert.NotNull(animal);
        Assert.Equal(1, animal.Id);
        Assert.Equal("Foo", animal.Name);
        Assert.Equal("Bar", animal.Type);
    }
}