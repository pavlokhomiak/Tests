using UnitTests.Units;
using Xunit;

namespace UnitTests.Tests;

public class TodoListTests
{
    [Fact]
    public void Add_SavesTodoItem()
    {
        TodoList todoList = new TodoList();
        
        todoList.Add(new TodoList.TodoItem("Test"));
        
        var item = Assert.Single(todoList.All);
        Assert.NotNull(item);
        Assert.Equal("Test", item.Content);
    }

    [Fact]
    public void TodoItemIdIncrementsEveryTimeWeAdd()
    {
        var list = new TodoList();
        
        list.Add(new TodoList.TodoItem("Test"));
        list.Add(new TodoList.TodoItem("Test"));

        var items = list.All.ToArray();
        
        Assert.Equal(1, items[0].Id);
        Assert.Equal(2, items[1].Id);
    }

    [Fact]
    public void Complete_SetsTodoItemCompleteFlagToTrue()
    {
        var list = new TodoList();
        list.Add(new TodoList.TodoItem("Test"));
        list.Complete(1);
        var item = Assert.Single(list.All);
        Assert.True(item.Complete);
    }
}