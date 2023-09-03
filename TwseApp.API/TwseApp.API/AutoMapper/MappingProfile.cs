using AutoMapper;
using TwseApp.API.Dto;
using TwseApp.API.Entities;

namespace TwseApp.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<CompanyData, Headquarters>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Code));
        
        }
    }
}
