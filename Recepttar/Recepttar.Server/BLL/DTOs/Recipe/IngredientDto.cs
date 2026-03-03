using Recepttar.Server.BLL.Enums;

namespace Recepttar.Server.BLL.DTOs.Recipe
{
    public class IngredientDto
    {
        public int Id { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public float Quantity { get; set; }
        public MeasurementUnitEnum MeasurementUnit { get; set; }
    }
}
