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
        public DbSet<Favorite> Favorite { get; set; }
        public DbSet<Vote> Vote { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Difficulty)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<Enums.RecipeDiffEnum>(v)
                );

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<Enums.RecipeTypeEnum>(v)
                );
        }

    }
}
