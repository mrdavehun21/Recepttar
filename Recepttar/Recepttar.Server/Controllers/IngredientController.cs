using Microsoft.AspNetCore.Mvc;
using Recepttar.Server.Models;

namespace Recepttar.Server.Controllers
{
    [ApiController()]
    [Route("ingredients/")]
    public class IngredientController : Controller
    {
        private readonly AppDbContext _context;
        public IngredientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public IActionResult SearchTags([FromQuery]string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                var res = _context.Ingredient.ToList().GetRange(0, 4);
                return Ok(res);
            }
            else
            {
                var res = _context.Ingredient.Where(d => d.Name.Contains(search)).ToList();
                return Ok(res.GetRange(0, Math.Min(res.Count, 4)));
            }
        }

        [HttpGet("units")]
        public IActionResult GetUnits()
        {
            List<string> units = new List<string>();
            foreach(var unit in Enum.GetValues<Enums.MeasurementUnitEnum>())
            {
                units.Add(unit.ToString());
            }
            return Ok(units);
        }
    }
}
