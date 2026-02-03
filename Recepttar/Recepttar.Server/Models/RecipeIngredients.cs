using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class RecipeIngredients
    {
        [Key]
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int IngredientId { get; set; }
        public Ingredients Ingredient { get; set; }

        public float Quantity { get; set; }
        public Enums.MeasurementUnitEnum MeasurementUnit { get; set; }
    }
}
