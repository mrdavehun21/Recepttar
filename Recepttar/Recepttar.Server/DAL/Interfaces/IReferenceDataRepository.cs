using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Interfaces
{
    public interface IReferenceDataRepository
    {
        Task<List<Ingredient>> SearchAsync(string? search);
    }
}
