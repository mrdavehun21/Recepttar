namespace Recepttar.Server.DTO.RecipeDTO
{
    public class IngredientsDTO
    {
        public int Id { get; set; }
        public string? IngredientName { get; set; }
        public float Quantity { get; set; }
        public Enums.MeasurementUnitEnum MeasurementUnit { get; set; }
    }
}
