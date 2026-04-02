using AutoMapper;
using Recepttar.Server.DAL.Models;
using Recepttar.Server.BLL.HelperMethods;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.User;
using Recepttar.Server.BLL.Enums;

namespace Recepttar.Server.BLL.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, ProfileDto>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => PicturePaths.ProfilePicturePath.GetPath(src.Id)));

            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(_ => string.Empty))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(_ => Array.Empty<byte>()))
                .ForMember(dest => dest.Rank, opt => opt.MapFrom(_ => UserRanksEnum.HomeCook));
        }
    }
}
