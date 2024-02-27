using AutoMapper;
using BusinessEntities;
using WebApi.DTOs.Clients;
using WebApi.DTOs.Specializations;

namespace WebApi.Mappings
{
    public class SpecsProfile : Profile
    {
        public SpecsProfile()
        {
            CreateMap<Specialization, SpecializationDTO>();
            CreateMap<SpecializationCreateDTO, Specialization>();
            CreateMap<SpecializationEditDTO, Specialization>();
        }
    }
}
