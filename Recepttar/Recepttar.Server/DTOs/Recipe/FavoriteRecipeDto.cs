namespace Recepttar.Server.DTOs.Recipe
{
    public class FavoriteRecipeDto
    {
        public int RecipeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DishPicture { get; set; }
    }
}
