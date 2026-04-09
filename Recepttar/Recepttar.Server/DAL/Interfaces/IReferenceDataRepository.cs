using Recepttar.Server.BLL.Enums;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Interfaces
{
    public interface IReferenceDataRepository
    {
        Task<IEnumerable<Ingredient>> SearchAsync(string? search, LanguagesEnum? language);
    }
}
