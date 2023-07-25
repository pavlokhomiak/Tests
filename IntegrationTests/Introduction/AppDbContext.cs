using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Introduction;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Animal> Animals { get; set; }
}