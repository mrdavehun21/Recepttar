namespace Recepttar.Server.DTO
{
    // DTO stands for Data Transfer Object
    // Also, make them nullable (? mark) will make them not required
    public class SearchQueries
    {
        public Enums.RecipeTypeEnum? Type { get; set; }
        public Enums.RecipeDiffEnum? Difficulty { get; set; }
        public bool? IsVegan { get; set; }
        public bool? IsExpensive { get; set; }
        public string? Search { get; set; }
        public string? Ingredients { get; set; }
    }
}
