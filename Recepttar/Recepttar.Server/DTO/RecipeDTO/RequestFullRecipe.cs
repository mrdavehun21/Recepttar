namespace Recepttar.Server.DTO.RecipeDTO
{
    public class RequestFullRecipe : CommonRecipe
    {
        public string DishPicture { get; set; }
        public List<DTO.RecipeDTO.IngredientsDTO>? Ingredients { get; set; }
        public List<DTO.RecipeDTO.RecipeStepsDTO> RecipeSteps { get; set; }
    }
}
