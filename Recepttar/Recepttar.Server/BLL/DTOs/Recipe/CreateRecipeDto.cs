using Recepttar.Server.BLL.Enums;

namespace Recepttar.Server.BLL.DTOs.Recipe
{
    public class CreateRecipeDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RecipeDiffEnum Difficulty { get; set; }
        public int TimeMinutes { get; set; }
        public int Servings { get; set; }
        public bool IsExpensive { get; set; }
        public bool IsVegan { get; set; }
        public RecipeTypeEnum Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public IFormFile DishPicture { get; set; } = null!;

        public List<IngredientDto> Ingredients { get; set; } = new();
        public List<StepDto> Steps { get; set; } = new();
    }
}
