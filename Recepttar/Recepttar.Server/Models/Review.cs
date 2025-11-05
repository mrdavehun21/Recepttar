using System.ComponentModel.DataAnnotations;

namespace Recepttar.Server.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int UserId { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
