namespace Recepttar.Server.DTO.RecipeDTO
{
    public class RequestFullRecipe : CommonRecipe
    {
        public int Id { get; set; }
        public string DishPicture { get; set; }
    }
}
