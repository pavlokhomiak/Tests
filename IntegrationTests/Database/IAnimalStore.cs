namespace IntegrationTests.Database;

public interface IAnimalStore
{
    Task<IList<Animal>> GetAnimals();
    Task<Animal> GetAnimal(int id);
    Task SaveAnimal(Animal animal);
}