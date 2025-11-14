using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class Favorites
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } // Navigation property
    }
}