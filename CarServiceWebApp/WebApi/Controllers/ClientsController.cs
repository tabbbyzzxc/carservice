using BusinessEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        [HttpGet("clients-list")]
        public async Task<List<Client>> GetClients()
        {
            return new List<Client>
            {
                new Client { Id = 1, Name = "Vasya"},
                new Client { Id = 2, Name = "Petya"}
            };
        }
    }
}
