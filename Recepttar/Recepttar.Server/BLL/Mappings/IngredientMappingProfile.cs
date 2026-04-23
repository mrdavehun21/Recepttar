using AutoMapper;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Mappings
{
    public class IngredientMappingProfile : Profile
    {
        public IngredientMappingProfile()
        {
            CreateMap<Ingredient, IngredientSearchDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, _, context) =>
                    context.Items["lang"]?.ToString() == "hu"
                        ? src.HuName
                        : src.Name
                ));
        }
    }
}
