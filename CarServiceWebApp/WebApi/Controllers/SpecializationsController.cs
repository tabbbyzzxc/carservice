using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessEntities;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs.Cars;
using WebApi.DTOs.Clients;
using WebApi.DTOs.Specializations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private CarsDbContext _context;
        private IMapper _mapper;

        public SpecializationsController(CarsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("specs-list")]
        public async Task<SpecializationListDTO> GetSpecs([FromQuery] int skip, [FromQuery] int take, [FromQuery] string? search = null)
        {
            var query = _context.Specializations.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = $"%{search.Replace("%", "[%]")}%";
                query = query.Where(c => EF.Functions.ILike(c.Name, search));
            }

            var total = await query
                .Where(c => c.IsActive == true)
                .CountAsync();
            var specs = await query
                .Where(c => c.IsActive == true)
                .OrderBy(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ProjectTo<SpecializationDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();


            return new SpecializationListDTO { Specializations = specs, Total = total };
        }

        [HttpGet("get-spec/{specId:long}")]
        public async Task<SpecializationDTO> GetSpec([FromRoute] long specId)
        {
            var query = _context.Specializations.AsNoTracking();
            var spec = query
                .Where(c => c.IsActive == true)
                .ProjectTo<SpecializationDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefault(s => s.Id == specId);

            return spec;
        }

        [HttpPost("post-spec")]
        public async Task<SpecializationListDTO> PostSpec(SpecializationCreateDTO newSpecDTO)
        {
            Specialization newSpec = _mapper.Map<Specialization>(newSpecDTO);
            await _context.Specializations.AddAsync(newSpec);
            await _context.SaveChangesAsync();

            var specs = await _context.Specializations.AsNoTracking()
                .ProjectTo<SpecializationDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new SpecializationListDTO { Specializations = specs };
        }

        [HttpPut("edit-spec/{specId:long}")]
        public async Task<IActionResult> EditSpec([FromRoute] long specId, [FromBody] SpecializationEditDTO editedSpecDTO)
        {
            if (editedSpecDTO == null)
            {
                return BadRequest("Invalid data");
            }

            var spec = await _context.Specializations.FindAsync(specId);

            if (spec == null)
            {
                return NotFound("Car not found");
            }

            _mapper.Map(editedSpecDTO, spec);

            _context.Specializations.Update(spec);
            await _context.SaveChangesAsync();


            return Ok(new { Message = "Data received successfully", Data = editedSpecDTO });
        }

        [HttpDelete("delete-spec/{specId:long}")]
        public async Task<IActionResult> DeleteSpec([FromRoute] long specId)
        {
            if (specId == null)
            {
                return BadRequest("Invalid data");
            }

            var spec = await _context.Specializations.FindAsync(specId);

            if (spec == null)
            {
                return NotFound("Spec not found");
            }

            spec.IsActive = false;

            _context.Specializations.Update(spec);
            await _context.SaveChangesAsync();


            return Ok(new { Message = "Car deleted successfully" });
        }
    }
}
