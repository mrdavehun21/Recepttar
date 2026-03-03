using AutoMapper;
using Recepttar.Server.BLL.DTOs.Poll;
using Recepttar.Server.DAL.Models;

namespace Recepttar.Server.BLL.Mappings
{
    public class PollMappingProfile : Profile
    {
        public PollMappingProfile()
        {
            CreateMap<Poll, PollDto>()
                .ForMember(dest => dest.Options, opt => opt.Ignore())
                .ForMember(dest => dest.VotedOn, opt => opt.Ignore());

            CreateMap<PollOptionDto, PollOption>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PollId, opt => opt.Ignore());
        }
    }
}
