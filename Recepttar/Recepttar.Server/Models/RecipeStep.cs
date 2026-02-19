using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recepttar.Server.Models
{
    public class RecipeStep
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }

        public int StepNumber { get; set; }

        [Required]
        public string StepDescription { get; set; } = string.Empty;

        // Navigation
        public Recipe Recipe { get; set; } = null!;
    }
}
