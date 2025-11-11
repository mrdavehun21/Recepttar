namespace Recepttar.Server.DTO.RecipeDTO
{
    public class FavoriteRecipe
    {
        public string Title { get; set; }
        public Enums.RecipeDiffEnum Difficulty { get; set; }
        public int TimeMinutes { get; set; }
        public int Servings { get; set; }
        public string DishPicture { get; set; }
    }
}
