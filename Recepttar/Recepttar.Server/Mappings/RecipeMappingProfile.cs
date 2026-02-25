using AutoMapper;
using Recepttar.Server.DTOs.Recipe;
using Recepttar.Server.Models;

namespace Recepttar.Server.Mappings
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {
            CreateMap<CreateRecipeDto, Recipe>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.DishPicture, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore())
                .ForMember(dest => dest.Steps, opt => opt.Ignore());

            CreateMap<IngredientDto, RecipeIngredient>()
                .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RecipeId, opt => opt.Ignore());

            CreateMap<StepDto, RecipeStep>().ForMember(dest => dest.RecipeId, opt => opt.Ignore());
        }
    }
}
