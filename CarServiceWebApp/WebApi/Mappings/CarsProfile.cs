using AutoMapper;
using BusinessEntities;
using WebApi.DTOs.Cars;

namespace WebApi.Mappings
{
    public class CarsProfile : Profile
    {
        public CarsProfile()
        {
            CreateMap<Car, CarDTO>();
            CreateMap<CarCreateDTO, Car>();
            CreateMap<CarEditDTO, Car>();
        }
    }
}
