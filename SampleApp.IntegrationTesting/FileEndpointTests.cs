using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Controllers;
using Xunit;

namespace SampleApp.IntegrationTesting;

public class FileEndpointTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public FileEndpointTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Configure<FileSettings>(fs =>
                {
                    // changes the default path to from "files" to "test_files"
                    fs.Path = "test_files";
                });
            });
        });
    }

    [Fact]
    public async void SavesFileToDisk()
    {
        var client = _factory.CreateClient();

        MultipartFormDataContent form = new();
        var file = await GetTestFile();
        form.Add(new StreamContent(file), "file", "test.txt");
        var response = await client.PostAsync("/api/files", form);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var fileResponse = await client.GetAsync("/test_files/test.txt");
        Assert.Equal(HttpStatusCode.OK, fileResponse.StatusCode);
    }

    public async Task<Stream> GetTestFile()
    {
        var memoryStream = new MemoryStream();
        var fileStream = File.OpenRead("test.txt");
        await fileStream.CopyToAsync(memoryStream);
        fileStream.Close();
        return memoryStream;
    }
}