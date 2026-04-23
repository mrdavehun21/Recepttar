using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.BLL.Enums;

namespace Recepttar.Server.BLL.Interfaces
{
    public interface IReferenceDataService
    {
        Task<IEnumerable<IngredientSearchDto>> SearchTagsAsync(string? search, LanguagesEnum? language);
        IEnumerable<string> GetUnits();
    }
}
