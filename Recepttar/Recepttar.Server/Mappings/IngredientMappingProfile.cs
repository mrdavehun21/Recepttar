using AutoMapper;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Models;

namespace Recepttar.Server.Mappings
{
    public class IngredientMappingProfile : Profile
    {
        public IngredientMappingProfile()
        {
            CreateMap<Ingredient, IngredientSearchDto>();
        }
    }
}
