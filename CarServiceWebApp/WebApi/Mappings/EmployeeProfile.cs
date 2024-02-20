using AutoMapper;
using BusinessEntities;
using WebApi.DTOs.Clients;
using WebApi.DTOs.Employees;

namespace WebApi.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeCreateDTO, Employee>();
            CreateMap<EmployeeEditDTO, Employee>();
        }
    }
}
