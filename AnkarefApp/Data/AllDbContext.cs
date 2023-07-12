namespace AnkarefApp.Data;

using Microsoft.EntityFrameworkCore;

public class AllDbContext : DbContext
{
    public AllDbContext(DbContextOptions<AllDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
}
