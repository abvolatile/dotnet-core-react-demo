using AutoMapper;
using Domain;

namespace Application.Utilities
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>(); //we get this method from AutoMapper
        }
    }
}