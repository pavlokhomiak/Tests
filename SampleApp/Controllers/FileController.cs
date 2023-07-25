using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SampleApp.Controllers;

[ApiController]
[Route("/api/files")]
public class FileController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly FileSettings _settings;

    public FileController(
        IWebHostEnvironment env,
        IOptionsMonitor<FileSettings> optionsMonitor
    )
    {
        _env = env;
        _settings = optionsMonitor.CurrentValue;
    }

    [HttpPost]
    public async Task<IActionResult> SaveFile([FromForm] IFormFile file)
    {
        // if no wwwroot folder exists, create one
        // if (string.IsNullOrWhiteSpace(_env.WebRootPath))
        // {
        //     _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        // }
        
        var savePath = Path.Combine(_env.WebRootPath, _settings.Path);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        var fileSavePath = Path.Combine(savePath, file.FileName);
        await using var fileStream = System.IO.File.Create(fileSavePath);
        await file.CopyToAsync(fileStream);
        return Ok();
    }
}