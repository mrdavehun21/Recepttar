using Recepttar.Server.DTOs.User;
using Recepttar.Server.Models;
using Recepttar.Server.HelperMethods;
using Recepttar.Server.Constants;
using Recepttar.Server.Enums;
using AutoMapper;

namespace Recepttar.Server.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, ProfileDto>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => ProfilePicturePath.GetPath(src.Id)));

            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => PasswordHash.PasswordHasher(src.Password)))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(_ => string.Empty))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(_ => Array.Empty<byte>()))
                .ForMember(dest => dest.Rank, opt => opt.MapFrom(_ => UserRanksEnum.HomeCook));
        }
    }
}
