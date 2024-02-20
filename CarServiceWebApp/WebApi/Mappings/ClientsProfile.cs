using AutoMapper;
using BusinessEntities;
using WebApi.DTOs.Clients;

namespace WebApi.Mappings
{
    public class ClientsProfile : Profile
    {
        public ClientsProfile()
        {
            CreateMap<Client, ClientDTO>();
            CreateMap<ClientCreateDTO, Client>();
            CreateMap<ClientEditDTO, Client>();
        }
    }
}
