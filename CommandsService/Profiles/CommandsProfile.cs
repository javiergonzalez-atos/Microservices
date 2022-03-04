using CommandsService.Models;
using CommandsService.Dtos;
using PlatformService;
using AutoMapper;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(destination => destination.ExternalID, 
                    opt=> opt.MapFrom(src => src.Id));
            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(dest => dest.ExternalID, 
                    opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(dest => dest.Commands, opt=> opt.Ignore());
        }
    }
}