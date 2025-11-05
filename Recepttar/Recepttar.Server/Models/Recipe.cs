using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recepttar.Server.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Enums.RecipeDiffEnum Difficulty { get; set; }
        public int TimeMinutes { get; set; }
        public int Servings { get; set; }
        public bool IsExpensive { get; set; }
        public bool IsVegan { get; set; }
        public Enums.RecipeTypeEnum Type { get; set; }
        public byte[] DishPicture { get; set; }

        public int AuthorId { get; set; } // Foreign Key
        public User Author { get; set; } // Navigation property
        // AuthorId - Author = Id, which is the name of the User primary key column
    }
}
