using AutoMapper;
using Recepttar.Server.DTOs.Poll;
using Recepttar.Server.Models;

namespace Recepttar.Server.Mappings
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
