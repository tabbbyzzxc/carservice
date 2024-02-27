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
        public async Task<ClientListDTO> GetClients([FromQuery] int skip, [FromQuery] int take = 15, [FromQuery] string? search = null)
        {
            var query = _context.Clients.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = $"%{search.Replace("%", "[%]")}%";
                query = query.Where(c => EF.Functions.ILike(c.FirstName, search) || EF.Functions.ILike(c.LastName, search));
            }

            var total = await query
                .Where(c => c.IsActive)
                .CountAsync();
            var clients = await query
                .Where(c => c.IsActive)
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName)
                .Include(x => x.Cars.Where(car => car.IsActive))
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            var clientsDto = _mapper.Map<List<ClientDTO>>(clients);

            return new ClientListDTO { Clients = clientsDto, Total = total };
        }

        [HttpGet("get-client")]
        public async Task<ClientDTO> GetClient([FromQuery] int id)
        {
            var query = _context.Clients.AsNoTracking();
            var client = query
                .Where(c => c.IsActive)
                .Include(x => x.Cars.Where(car => car.IsActive))
                .FirstOrDefault(c => c.Id == id);

            return _mapper.Map<ClientDTO>(client);
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

        [HttpDelete("delete-client/{clientId:long}")]
        public async Task<IActionResult> DeleteClient([FromRoute] long clientId)
        {
            if (clientId == null)
            {
                return BadRequest("Invalid data");
            }

            var client = await _context.Clients.FindAsync(clientId);

            if (client == null)
            {
                return NotFound("Client not found");
            }

            client.IsActive = false;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Client deleted successfully" });
        }
    }
}
