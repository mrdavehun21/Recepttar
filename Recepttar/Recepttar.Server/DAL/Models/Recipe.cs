using Recepttar.Server.BLL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recepttar.Server.DAL.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public RecipeDiffEnum Difficulty { get; set; }

        public int TimeMinutes { get; set; }

        public int Servings { get; set; }

        public bool IsExpensive { get; set; }

        public bool IsVegan { get; set; }

        public RecipeTypeEnum Type { get; set; }

        [Required]
        public byte[] DishPicture { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        // Navigation
        public User Author { get; set; } = null!;
        public ICollection<RecipeStep> Steps { get; set; } = new List<RecipeStep>();
        public ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
