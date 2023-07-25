using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Controllers;
using Xunit;

namespace SampleApp.IntegrationTesting;

// WebApplicationFactory - make mock instance of application
// and make API call to it by http client
public class AnimalEndpointTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public AnimalEndpointTests(WebApplicationFactory<Startup> factory)
    {
        // change Sturtup.cs to use AnimalServiceMock instead of AnimalService
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IAnimalService, AnimalServiceMock>();

                // remove AnimalService from services
                // var animalService = services.Single(x => x.ServiceType == typeof(IAnimalService));
                // services.Remove(animalService);
            });
        });
    }

    public class AnimalServiceMock : IAnimalService
    {
        public Animal GetAnimal()
        {
            return new()
            {
                Id = 2,
                Name = "Foo2",
                Type = "Bar2",
            };
        }
    }

    [Fact]
    public async Task GetsAnimal()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/animals");
        var animal = await response.Content.ReadFromJsonAsync<Animal>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(animal);
        Assert.Equal(2, animal.Id);
    }
}