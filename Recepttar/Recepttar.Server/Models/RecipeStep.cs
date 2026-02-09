using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class RecipeStep
    {
        [Key]
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int StepNumber { get; set; }
        public string StepDescription { get; set; }
    }
}
