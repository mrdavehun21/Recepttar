using Microsoft.EntityFrameworkCore;

namespace Recepttar.Server.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Poll> Poll { get; set; }
        public DbSet<PollOption> PollOption { get; set; }
    }
}
