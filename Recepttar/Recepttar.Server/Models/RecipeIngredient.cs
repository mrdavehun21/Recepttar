using Recepttar.Server.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recepttar.Server.Models
{
    public class RecipeIngredient
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }

        [ForeignKey(nameof(Ingredient))]
        public int IngredientId { get; set; }

        public float Quantity { get; set; }

        public MeasurementUnitEnum MeasurementUnit { get; set; }

        // Navigation
        public Recipe Recipe { get; set; } = null!;
        public Ingredient Ingredient { get; set; } = null!;
    }
}
