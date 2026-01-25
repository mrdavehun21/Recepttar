namespace Recepttar.Server.DTO.RecipeDTO
{
    public class CommonRecipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Enums.RecipeDiffEnum Difficulty { get; set; }
        public int TimeMinutes { get; set; }
        public int Servings { get; set; }
        public bool IsExpensive { get; set; }
        public bool IsVegan { get; set; }
        public Enums.RecipeTypeEnum Type { get; set; }
        public int AuthorId { get; set; }
    }
}
