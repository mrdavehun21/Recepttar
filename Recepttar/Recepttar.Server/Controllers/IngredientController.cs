using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Data;
using Recepttar.Server.Services;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly IngredientService _ingredientService;

        public IngredientController(IngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTags([FromQuery] string? search)
        {
            var res = await _ingredientService.SearchTagsAsync(search);

            return Ok(res);
        }

        [HttpGet("units")]
        public IActionResult GetUnits()
        {
            var units = _ingredientService.GetUnits();

            return Ok(units);
        }
    }
}
