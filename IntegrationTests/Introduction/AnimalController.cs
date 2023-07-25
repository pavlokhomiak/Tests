using Microsoft.AspNetCore.Mvc;

namespace IntegrationTests.Introduction;

public class AnimalController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public AnimalController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult List()
    {
        return Ok(_context.Animals.ToList());
    }
    
    [HttpGet]
    public IActionResult Get(int id)
    {
        return Ok(_context.Animals.FirstOrDefault(x => x.Id == id));
    }
    
}