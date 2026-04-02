using Microsoft.EntityFrameworkCore;
using Recepttar.Server.DAL.Models;
using Recepttar.Server.BLL.Enums;

namespace Recepttar.Server.DAL.Data
{
    public class RecepttarDbContext : DbContext
    {
        public RecepttarDbContext(DbContextOptions<RecepttarDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<RecipeStep> RecipeSteps => Set<RecipeStep>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<Poll> Polls => Set<Poll>();
        public DbSet<PollOption> PollOptions => Set<PollOption>();
        public DbSet<Vote> Votes => Set<Vote>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Unique (UserId, RecipeId) on Favourite
            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.RecipeId })
                .IsUnique();

            // Unique (UserId, RecipeId) on Review — one review per recipe per user
            modelBuilder.Entity<Review>()
                .HasIndex(r => new { r.UserId, r.RecipeId })
                .IsUnique();

            // Unique (UserId, OptionId) on Vote — prevents double voting per poll option
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.OptionId })
                .IsUnique();

            // Stars must be between 1 and 5
            modelBuilder.Entity<Review>()
                .ToTable(t => t.HasCheckConstraint("CK_Review_Stars", "`Stars` >= 1 AND `Stars` <= 5"));

            // Prevent cascade delete cycles on Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete cycles on Favourite
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete cycles on Vote
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enums
            modelBuilder.Entity<User>()
                .Property(r => r.Rank)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<UserRanksEnum>(v)
                )
                .HasColumnType($"ENUM('{string.Join("', '", Enum.GetNames<UserRanksEnum>())}')")
                .HasDefaultValue(UserRanksEnum.HomeCook);


            modelBuilder.Entity<Recipe>()
                .Property(r => r.Difficulty)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<RecipeDiffEnum>(v)
                )
                .HasColumnType($"ENUM('{string.Join("', '", Enum.GetNames<RecipeDiffEnum>())}')")
                .HasDefaultValue(RecipeDiffEnum.Easy);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<RecipeTypeEnum>(v)
                )
                .HasColumnType($"ENUM('{string.Join("', '", Enum.GetNames<RecipeTypeEnum>())}')")
                .HasDefaultValue(RecipeTypeEnum.Appetizer);

            modelBuilder.Entity<RecipeIngredient>()
                .Property(r => r.MeasurementUnit)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<MeasurementUnitEnum>(v)
                )
                .HasColumnType($"ENUM('{string.Join("', '", Enum.GetNames<MeasurementUnitEnum>())}')")
                .HasDefaultValue(MeasurementUnitEnum.Piece);
        }

    }
}
