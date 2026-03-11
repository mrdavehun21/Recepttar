using Microsoft.EntityFrameworkCore;
using Recepttar.Server.DAL.Data;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Repositories
{
    public class ReferenceDataRepository : IReferenceDataRepository
    {
        private readonly AppDbContext _context;

        private const int MaxSearchResults = 4;

        public ReferenceDataRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> SearchAsync(string? search)
        {
            var query = string.IsNullOrWhiteSpace(search)
                ? _context.Ingredients.Take(MaxSearchResults)
                : _context.Ingredients.Where(i => i.Name.Contains(search)).Take(4);

            return await query.ToListAsync();
        }
    }
}
