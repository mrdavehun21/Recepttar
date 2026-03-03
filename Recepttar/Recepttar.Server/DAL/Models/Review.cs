using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recepttar.Server.DAL.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }  

        [Required, MaxLength(1024)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public Recipe Recipe { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
