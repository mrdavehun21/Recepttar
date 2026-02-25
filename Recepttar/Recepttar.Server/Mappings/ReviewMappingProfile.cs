using AutoMapper;
using Recepttar.Server.Models;
using Recepttar.Server.DTOs.Review;

namespace Recepttar.Server.Mappings
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<AddReviewDto, Review>().ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));
        }
    }
}
