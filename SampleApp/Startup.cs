using SampleApp.Controllers;

namespace SampleApp;

public class Startup
{
    private readonly IConfiguration _config;

    public Startup(IConfiguration config)
    {
        _config = config;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.Configure<FileSettings>(_config.GetSection(nameof(FileSettings)));

        services.AddSingleton<IAnimalService, AnimalService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
    }
}