namespace UnitTests.Units;

public class CreateSomething
{
    private readonly IStore _store;

    public CreateSomething(IStore store)
    {
        _store = store;
    }

    public CreateSomethingResult Create(Something something)
    {
        // C#9 pattern matching
        if (something is { Name: { Length: > 0 } })
        {
            var isSomethingCreated = _store.Save(something);
            return new(isSomethingCreated);
        }

        return new(false, "Name is required");
    }

    public record CreateSomethingResult(bool Success, string Error = "");
    
    public interface IStore
    {
        bool Save(Something something);
    }
    
    public class Something
    {
        public string Name { get; set; }
        public int Age { get; }
        public DateTime Created { get; }
        public DateTime Updated { get; }
    }
}