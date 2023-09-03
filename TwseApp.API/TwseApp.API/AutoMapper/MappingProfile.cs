using AutoMapper;
using TwseApp.API.Entities;
using TwseApp.API.Models;

namespace TwseApp.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<CompanyData, Headquarters>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Code));
        
        }
    }
}
