namespace SampleApp.Controllers;

public class AnimalService : IAnimalService
{
    public Animal GetAnimal()
    {
        return new()
        {
            Id = 1,
            Name = "Foo",
            Type = "Bar",
        };
    }
}