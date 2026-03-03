namespace Recepttar.Server.BLL.DTOs.Recipe
{
    public class CreateFavoriteRecipeDto
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
    }
}
