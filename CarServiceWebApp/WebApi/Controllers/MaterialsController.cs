using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs.Materials;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private CarsDbContext _context;
        private IMapper _mapper;

        public MaterialsController(CarsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("materials-list")]
        public async Task<MaterialListDTO> GetMaterials([FromQuery] int skip, [FromQuery] int take, [FromQuery] string? search = null)
        {
            var query = _context.Materials.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = $"%{search.Replace("%", "[%]")}%";
                query = query.Where(m => EF.Functions.ILike(m.Name, search));
            }

            var total = await query.CountAsync();
            var materials = await query
                .Skip(skip)
                .Take(take)
                .ProjectTo<MaterialDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();


            return new MaterialListDTO { Materials = materials, Total = total };
        }
    }
}
