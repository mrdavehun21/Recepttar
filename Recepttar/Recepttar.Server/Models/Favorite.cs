using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } // Navigation property
    }
}