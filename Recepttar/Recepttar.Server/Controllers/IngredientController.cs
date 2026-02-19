using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Interfaces;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTags([FromQuery] string? search)
        {
            var foundTags = await _ingredientService.SearchTagsAsync(search);

            return Ok(foundTags);
        }

        [HttpGet("units")]
        public IActionResult GetUnits()
        {
            var units = _ingredientService.GetUnits();

            return Ok(units);
        }
    }
}
