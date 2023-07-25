using System.Reflection;
using IntegrationTests.Introduction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.Tests;

public class AnimalControllerTests
{
    [Fact]
    public void AnimalController_ListsAnimalsFromDatabase()
    {
        DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
        optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

        using (AppDbContext ctx = new(optionsBuilder.Options))
        {
            ctx.Add(new Animal { Name = "Dog", Type = "Foo"});
            ctx.SaveChanges();
        }
        
        IActionResult result;
        using (AppDbContext ctx = new(optionsBuilder.Options))
        {
            result = new AnimalController(ctx).List();
        }
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var animals = Assert.IsType<List<Animal>>(okResult.Value);
        var animal = Assert.Single(animals);
        Assert.Equal(1, animal.Id);
        Assert.Equal("Dog", animal.Name);
    }
    
    [Fact]
    public void AnimalController_GetsAnimalFromDatabase()
    {
        DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
        optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

        using (AppDbContext ctx = new(optionsBuilder.Options))
        {
            ctx.Add(new Animal { Name = "Dog", Type = "Foo"});
            ctx.SaveChanges();
        }
        
        IActionResult result;
        using (AppDbContext ctx = new(optionsBuilder.Options))
        {
            result = new AnimalController(ctx).Get(1);
        }
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var animal = Assert.IsType<Animal>(okResult.Value);
        Assert.Equal(1, animal.Id);
        Assert.Equal("Dog", animal.Name);
    }
}