using Recepttar.Server.BLL.DTOs.Recipe;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IReferenceDataService
    {
        Task<List<IngredientSearchDto>> SearchTagsAsync(string? search);
        List<string> GetUnits();
    }
}
