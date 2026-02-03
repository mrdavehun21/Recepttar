namespace Recepttar.Server.DTO.RecipeDTO
{
    public class CreateRecipe : CommonRecipe
    {
        public IFormFile DishPicture { get; set; }
        public List<DTO.RecipeDTO.IngredientsDTO> Ingredients { get; set; }
        public List<DTO.RecipeDTO.RecipeStepsDTO> RecipeSteps { get; set; }
    }
}
