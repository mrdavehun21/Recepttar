using Recepttar.Server.BLL.DTOs.Recipe;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IReferenceDataService
    {
        Task<IEnumerable<IngredientSearchDto>> SearchTagsAsync(string? search);
        IEnumerable<string> GetUnits();
    }
}
