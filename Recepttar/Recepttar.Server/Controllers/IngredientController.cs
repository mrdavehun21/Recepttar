using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.BLL.Interfaces;

namespace Recepttar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly IReferenceDataService _ingredientService;

        public IngredientController(IReferenceDataService ingredientService)
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
