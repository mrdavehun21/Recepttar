using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recepttar.Server.Data;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Interfaces.Repositories;

namespace Recepttar.Server.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public IngredientRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<IngredientSearchDto>> SearchTagsAsync(string? search)
        {
            var query = string.IsNullOrWhiteSpace(search)
                ? _context.Ingredients.Take(4)
                : _context.Ingredients.Where(i => i.Name.Contains(search)).Take(4);

            return _mapper.Map<List<IngredientSearchDto>>(await query.ToListAsync());
        }
    }
}
