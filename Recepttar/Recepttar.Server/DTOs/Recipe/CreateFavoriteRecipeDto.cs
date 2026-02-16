namespace Recepttar.Server.DTOs.Recipe
{
    public class CreateFavoriteRecipeDto
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
    }
}
