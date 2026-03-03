using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.DAL.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        // Navigation
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
