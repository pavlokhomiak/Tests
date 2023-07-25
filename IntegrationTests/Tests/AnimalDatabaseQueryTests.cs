using IntegrationTests.Database;
using Xunit;

namespace IntegrationTests.Tests;

[Collection(nameof(AnimalCollection))]
public class AnimalDatabaseQueryTests
{
    
    private readonly AnimalSetupFixture _animalSetupFixture;

    public AnimalDatabaseQueryTests(AnimalSetupFixture animalSetupFixture)
    {
        _animalSetupFixture = animalSetupFixture;
    }
    
    [Fact]
    public async void AnimalStore_ListsAnimalsFromDatabase()
    {
        IAnimalStore store = _animalSetupFixture.Store;

        var animals = await store.GetAnimals();

        Assert.True(animals.Count == 3);
        Assert.Contains(animals, a => a.Name == "Foo");
        Assert.Contains(animals, a => a.Name == "Bar");
        Assert.Contains(animals, a => a.Name == "Baz");
    }
}