using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessEntities;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs.Cars;
using WebApi.DTOs.Clients;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private CarsDbContext _context;
        private IMapper _mapper;

        public ClientsController(CarsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("clients-list")]
        public async Task<ClientListDTO> GetClients([FromQuery] int skip, [FromQuery] int take, [FromQuery] string? search = null)
        {
            var query = _context.Clients.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = $"%{search.Replace("%", "[%]")}%";
                query = query.Where(c => EF.Functions.ILike(c.FirstName, search) || EF.Functions.ILike(c.LastName, search));
            }

            var total = await query.CountAsync();
            var clients = await query
                .Include(x => x.Cars)
                .Skip(skip)
                .Take(take)
                .ProjectTo<ClientDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();


            return new ClientListDTO { Clients = clients, Total = total };
        }

        [HttpGet("get-client")]
        public async Task<ClientDTO> GetClient([FromQuery] int id)
        {
            var query = _context.Clients.AsNoTracking();
            var client = query
                .Include(x => x.Cars)/*.ProjectTo<CarDTO>(_mapper.ConfigurationProvider)*/
                .ProjectTo<ClientDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefault(c => c.Id == id);

            return client;
        }

        [HttpPost("post-client")]
        public async Task<IActionResult> PostClient([FromBody] ClientCreateDTO newClientDTO)
        {
            if (newClientDTO == null)
            {
                return BadRequest("Invalid data");
            }
            Client newClient = _mapper.Map<Client>(newClientDTO);
            await _context.Clients.AddAsync(newClient);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Data received successfully", Data = newClientDTO });
        }

        [HttpPut("edit-client/{clientId:long}")]
        public async Task<IActionResult> EditClient([FromRoute] long clientId, [FromBody] ClientEditDTO editedClientDTO)
        {
            if (editedClientDTO == null)
            {
                return BadRequest("Invalid data");
            }

            var client = await _context.Clients.FindAsync(clientId);

            if (client == null)
            {
                return NotFound("Client not found");
            }

            _mapper.Map(editedClientDTO, client);

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Data received successfully", Data = editedClientDTO });
        }
    }
}
