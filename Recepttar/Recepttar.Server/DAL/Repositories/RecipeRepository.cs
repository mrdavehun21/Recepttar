using Microsoft.EntityFrameworkCore;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.DAL.Data;
using Recepttar.Server.DAL.Interfaces;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.DAL.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly AppDbContext _context;

        public RecipeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _context.Recipes
                .Include(r => r.Reviews)
                .ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> GetByUserIdAsync(int userId)
        {
            return await _context.Recipes
                .Include(r => r.Reviews)
                .Where(r => r.AuthorId == userId)
                .ToListAsync();
        }

        public async Task<Recipe?> GetByIdAsync(int recipeId)
        {
            return await _context.Recipes
                .Include(r => r.Reviews)
                .Include(r => r.Ingredients).ThenInclude(ri => ri.Ingredient)
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == recipeId);
        }

        public async Task<Recipe> AddAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(recipe.Id);
        }

        public async Task UpdateAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Recipe recipe)
        {
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task ReplaceIngredientsAsync(int recipeId, IEnumerable<RecipeIngredient> ingredients)
        {
            await _context.RecipeIngredients.Where(ri => ri.RecipeId == recipeId).ExecuteDeleteAsync();
            _context.RecipeIngredients.AddRange(ingredients);
            await _context.SaveChangesAsync();
        }

        public async Task ReplaceStepsAsync(int recipeId, IEnumerable<RecipeStep> steps)
        {
            await _context.RecipeSteps.Where(rs => rs.RecipeId == recipeId).ExecuteDeleteAsync();
            _context.RecipeSteps.AddRange(steps);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Recipe>> SearchAsync(SearchQueryDto queryDto)
        {
            IQueryable<Recipe> query = _context.Recipes
                .Include(r => r.Reviews)
                .AsQueryable();

            if (queryDto.Type.HasValue)
            {
                query = query.Where(r => r.Type == queryDto.Type.Value);
            }

            if (queryDto.Difficulty.HasValue)
            {
                query = query.Where(r => r.Difficulty == queryDto.Difficulty.Value);
            }

            if (queryDto.IsVegan.HasValue)
            {
                query = query.Where(r => r.IsVegan == queryDto.IsVegan.Value);
            }

            if (queryDto.IsExpensive.HasValue)
            {
                query = query.Where(r => r.IsExpensive == queryDto.IsExpensive.Value);
            }

            if (!string.IsNullOrWhiteSpace(queryDto.Search))
            {
                query = query.Where(r => r.Title.Contains(queryDto.Search));
            }

            if (!string.IsNullOrWhiteSpace(queryDto.Ingredients))
            {
                var ingredientIds = queryDto.Ingredients
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                query = query
                    .Join(_context.RecipeIngredients, r => r.Id, ri => ri.RecipeId, (r, ri) => new { r, ri })
                    .Where(x => ingredientIds.Contains(x.ri.IngredientId))
                    .Select(x => x.r)
                    .Distinct();
            }

            return await query.ToListAsync();
        }
    }
}
