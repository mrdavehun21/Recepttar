namespace Recepttar.Server.DTOs.Recipe
{
    public class RecipeCardDto
    {
        public int RecipeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string DishPicture { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
