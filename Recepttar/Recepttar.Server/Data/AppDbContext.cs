using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Models;

namespace Recepttar.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }

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

            modelBuilder.Entity<User>()
                .Property(r => r.Rank)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<Enums.UserRanksEnum>(v)
                );
            modelBuilder.Entity<RecipeIngredient>()
                .Property(r => r.MeasurementUnit)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<Enums.MeasurementUnitEnum>(v)
                );
        }

    }
}
