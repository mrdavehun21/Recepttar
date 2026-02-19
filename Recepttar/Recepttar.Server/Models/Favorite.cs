using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recepttar.Server.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public Recipe Recipe { get; set; } = null!;
    }
}