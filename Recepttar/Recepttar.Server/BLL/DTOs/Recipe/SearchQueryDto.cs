namespace Recepttar.Server.BLL.DTOs.Recipe
{
    public class SearchQueryDto
    {
        public Enums.RecipeTypeEnum? Type { get; set; }
        public Enums.RecipeDiffEnum? Difficulty { get; set; }
        public bool? IsVegan { get; set; }
        public bool? IsExpensive { get; set; }
        public string? Search { get; set; }
        public string? Ingredients { get; set; }
    }
}
