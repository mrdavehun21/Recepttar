using AutoMapper;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Mappings
{
    public class IngredientMappingProfile : Profile
    {
        public IngredientMappingProfile()
        {
            CreateMap<Ingredient, IngredientSearchDto>();
        }
    }
}
