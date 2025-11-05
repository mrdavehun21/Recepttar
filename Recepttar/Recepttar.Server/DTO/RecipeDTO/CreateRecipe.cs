namespace Recepttar.Server.DTO.RecipeDTO
{
    public class CreateRecipe : CommonRecipe
    {
        public IFormFile DishPicture { get; set; }
    }
}
