using Moq;
using UnitTests.Units;
using Xunit;

namespace UnitTests.Tests;

public class CreateSomethingTests
{
    
    private readonly Mock<CreateSomething.IStore> _storeMock = new();
    
    [Fact]
    public void CreateSomethingResult_NotSuccess_WhenSomethingIsNotCreated()
    {
        CreateSomething createSomething = new(_storeMock.Object);
        var result = createSomething.Create(null);
        
        Assert.False(result.Success);
        _storeMock.Verify(x => x.Save(It.IsAny<CreateSomething.Something>()), Times.Never);
    }
    
    [Fact]
    public void CreateSomethingResult_Success_WhenSomethingIsCreated()
    {
        CreateSomething createSomething = new(_storeMock.Object);
        CreateSomething.Something something = new CreateSomething.Something { Name = "Foo" };
        
        _storeMock
            .Setup(x => x.Save(something))
            .Returns(true);
        
        var result = createSomething.Create(something);
        
        Assert.True(result.Success);
        _storeMock.Verify(x => x.Save(something), Times.Once);

    }
}