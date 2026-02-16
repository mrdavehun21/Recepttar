using Recepttar.Server.Enums;

namespace Recepttar.Server.DTOs.Recipe
{
    public class UpdateRecipeDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public RecipeDiffEnum? Difficulty { get; set; }
        public int? TimeMinutes { get; set; }
        public int? Servings { get; set; }
        public bool? IsExpensive { get; set; }
        public bool? IsVegan { get; set; }
        public RecipeTypeEnum? Type { get; set; }
        public IFormFile? DishPicture { get; set; }

        public List<IngredientDto>? Ingredients { get; set; }
        public List<StepDto>? RecipeSteps { get; set; }
    }
}
