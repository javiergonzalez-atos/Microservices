using PlatformService.Models;
using PlatformService.Dto;
using AutoMapper;

namespace PlatformService.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(destination => destination.PlatformId, 
                    opt => opt.MapFrom(src => src.Id));
        }
    }
}