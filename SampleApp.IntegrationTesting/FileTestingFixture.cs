using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Controllers;
using Xunit;

namespace SampleApp.IntegrationTesting;

// IAsyncLifetime runs InitializeAsync() before each test and DisposeAsync() after each test
public class FileTestingFixture : WebApplicationFactory<Startup>, IAsyncLifetime
{
    public Stream TestFile { get; private set; }
    private string _cleanupPath;

    public async Task InitializeAsync()
    {
        TestFile = await GetTestFile();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(services =>
        {
            services.Configure<FileSettings>(fs =>
            {
                fs.Path = "test_files";
            });

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            // get IWebHostEnvironment from service container
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            _cleanupPath = Path.Combine(env.WebRootPath, "test_files");
        });
    }

    private async Task<Stream> GetTestFile()
    {
        var memoryStream = new MemoryStream();
        var fileStream = File.OpenRead("test.txt");
        await fileStream.CopyToAsync(memoryStream);
        fileStream.Close();
        return memoryStream;
    }
    
    // delete test_files directory in SampleApp > wwwroot after each test
    public Task DisposeAsync()
    {
        var directoryInfo = new DirectoryInfo(_cleanupPath);
        foreach (var file in directoryInfo.GetFiles())
        {
            file.Delete();
        }

        Directory.Delete(_cleanupPath);
        return Task.CompletedTask;
    }
}