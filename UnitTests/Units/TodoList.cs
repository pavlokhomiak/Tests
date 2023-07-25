namespace UnitTests.Units;

public class TodoList
{
    public record TodoItem(string Content)
    {
        public int Id { get; set; }
        public bool Complete { get; set; }
    }

    private readonly List<TodoItem> _todoItems = new();
    private int idCounter = 1;

    public void Add(TodoItem item)
    {
        _todoItems.Add(item with { Id = idCounter++ });
    }

    public IEnumerable<TodoItem> All => _todoItems;

    public void Complete(int id)
    {
        var item = _todoItems.First(x => x.Id == id);
        _todoItems.Remove(item);
        
        var completedItem = item with {Id = idCounter++, Complete = true};
        _todoItems.Add(completedItem);
    }
}