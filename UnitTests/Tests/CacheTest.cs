using UnitTests.Units;
using Xunit;

namespace UnitTests.Tests;

public class CacheTest
{
    [Fact]
    public void Contains_ReturnsTrue_WithinTimeSpan()
    {
        var cache = new Cache(TimeSpan.FromDays(1));
        cache.Add(new ("url", "content", DateTime.Now));

        bool contains = cache.Contains("url");
        
        Assert.True(contains);
    }
    
    [Fact]
    public void Contains_ReturnsFalse_WhenOutsideTimeSpan()
    {
        var cache = new Cache(TimeSpan.FromDays(1));
        cache.Add(new ("url", "content", DateTime.Now.Subtract(TimeSpan.FromDays(2))));

        var contains = cache.Contains("url");
        
        Assert.False(contains);
    }
    
    [Fact]
    public void Contains_ReturnsFalse_WhenDoesntContainItem()
    {
        var cache = new Cache(TimeSpan.FromDays(1));

        var contains = cache.Contains("url");
        
        Assert.False(contains);
    }
}