using AutoMapper;
using Recepttar.Server.BLL.Constants;
using Recepttar.Server.BLL.DTOs.Review;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Mappings
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<AddReviewDto, Review>().ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => ProfilePicturePath.GetPath(src.User.Id)));
        }
    }
}
