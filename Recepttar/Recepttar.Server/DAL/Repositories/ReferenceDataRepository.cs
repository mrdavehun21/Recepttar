using Microsoft.EntityFrameworkCore;
using Recepttar.Server.BLL.Enums;
using Recepttar.Server.DAL.Data;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Repositories
{
    public class ReferenceDataRepository : IReferenceDataRepository
    {
        private readonly RecepttarDbContext _context;

        private const int MaxSearchResults = 4;

        public ReferenceDataRepository(RecepttarDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> SearchAsync(string? search, LanguagesEnum? language)
        {
            IQueryable<Ingredient> query;
            switch (language)
            {
                case LanguagesEnum.en:
                    query = string.IsNullOrWhiteSpace(search)
                        ? _context.Ingredients.Take(MaxSearchResults)
                        : _context.Ingredients.Where(i => i.Name.Contains(search)).Take(4);
                    break;
                case LanguagesEnum.hu:
                    query = string.IsNullOrWhiteSpace(search)
                        ? _context.Ingredients.Take(MaxSearchResults)
                        : _context.Ingredients.Where(i => i.HuName.Contains(search)).Take(4);
                    break;
                default:
                    query = string.IsNullOrWhiteSpace(search)
                        ? _context.Ingredients.Take(MaxSearchResults)
                        : _context.Ingredients.Where(i => i.Name.Contains(search)).Take(4);
                    break;
            }

            return await query.ToListAsync();
        }
    }
}
