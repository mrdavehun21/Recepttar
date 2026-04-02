using AutoMapper;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Leaderboard;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Mappings
{
    public class LeaderboardMappingProfile : Profile
    {
        public LeaderboardMappingProfile()
        {
            CreateMap<User, LeaderboardEntryDto>()
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProfilePicture,
                    opt => opt.MapFrom(src =>
                        ProfilePicturePath.GetPath(src.Id)))
                .ForMember(dest => dest.RecipeCount,
                    opt => opt.MapFrom(src => src.Recipes.Count))
                .ForMember(dest => dest.AvgRating,
                    opt => opt.MapFrom(src =>
                        src.Recipes.SelectMany(r => r.Reviews).Any()
                            ? (float)Math.Round(src.Recipes.SelectMany(r => r.Reviews)
                                .Average(rv => (double?)rv.Stars) ?? 0, 1)
                            : 0f))
                .ForMember(dest => dest.FavoriteCount,
                    opt => opt.MapFrom(src => src.Recipes
                        .SelectMany(r => r.Favorites).Count()));
        }
    }
}
