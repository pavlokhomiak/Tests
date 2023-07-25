using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Controllers;
using Xunit;

namespace SampleApp.IntegrationTesting;

public class FileEndpointTests : IClassFixture<FileTestingFixture>
{
    private readonly FileTestingFixture _fixture;

    public FileEndpointTests(FileTestingFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async void SavesFileToDisk()
    {
        var client = _fixture.CreateClient();

        MultipartFormDataContent form = new();
        var file = _fixture.TestFile;
        form.Add(new StreamContent(file), "file", "test.txt");
        var response = await client.PostAsync("/api/files", form);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var fileResponse = await client.GetAsync("/test_files/test.txt");
        Assert.Equal(HttpStatusCode.OK, fileResponse.StatusCode);
    }
}