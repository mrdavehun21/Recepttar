using AutoMapper;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Recipe;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Mappings
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {
            CreateMap<Recipe, RecipeCardDto>()
                .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DishPicture, opt => opt.MapFrom(src => PicturePaths.DishPicturePath.GetPath(src.Id)))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                    src.Reviews.Any()
                        ? (float)Math.Round(src.Reviews.Average(rv => rv.Stars), 1)
                        : 0f))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count));

            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DishPicture, opt => opt.MapFrom(src => PicturePaths.DishPicturePath.GetPath(src.Id)))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
                .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps.OrderBy(s => s.StepNumber)));

            CreateMap<RecipeIngredient, IngredientDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IngredientId))
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient.Name));

            CreateMap<RecipeStep, StepDto>();

            CreateMap<CreateRecipeDto, Recipe>()
                .ForMember(dest => dest.DishPicture, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore())
                .ForMember(dest => dest.Steps, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<IngredientDto, RecipeIngredient>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.Id));

            CreateMap<StepDto, RecipeStep>();
        }
    }
}
