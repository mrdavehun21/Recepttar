using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Data;
using Recepttar.Server.Enums;
using Recepttar.Server.Interfaces;
using Recepttar.Server.Models;

namespace Recepttar.Server.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly AppDbContext _context;
        public IngredientService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ingredient>> SearchTagsAsync(string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return await _context.Ingredients.Take(4).ToListAsync();
            }

            return await _context.Ingredients.Where(i => i.Name.Contains(search)).Take(4).ToListAsync();
        }

        public List<string> GetUnits()
        {
            return Enum.GetValues<MeasurementUnitEnum>().Select(u => u.ToString()).ToList();
        }
    }
}
