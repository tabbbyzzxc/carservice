using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessEntities;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs.Clients;
using WebApi.DTOs.Employees;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private CarsDbContext _context;
        private IMapper _mapper;

        public EmployeesController(CarsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("employees-list")]
        public async Task<EmployeeListDTO> GetEmployees([FromQuery] int skip, [FromQuery] int take, [FromQuery] string? search = null)
        {
            var query = _context.Employees.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = $"%{search.Replace("%", "[%]")}%";
                query = query.Where(c => EF.Functions.ILike(c.FirstName, search) || EF.Functions.ILike(c.LastName, search));
            }

            var total = await query.CountAsync();
            var employees = await query
                .Skip(skip)
                .Take(take)
                .ProjectTo<EmployeeDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();


            return new EmployeeListDTO { Employees = employees, Total = total };
        }

        [HttpGet("get-employee")]
        public async Task<EmployeeDTO> GetEmployee([FromQuery] int id)
        {
            var query = _context.Employees.AsNoTracking();
            var employee = query
                .ProjectTo<EmployeeDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefault(c => c.Id == id);

            return employee;
        }

        [HttpPost("post-employee")]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeCreateDTO newEmployeeDTO)
        {
            if (newEmployeeDTO == null)
            {
                return BadRequest("Invalid data");
            }
            Employee newEmployee = _mapper.Map<Employee>(newEmployeeDTO);
            await _context.Employees.AddAsync(newEmployee);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Data received successfully", Data = newEmployeeDTO });
        }

        [HttpPut("edit-employee/{employeeId:long}")]
        public async Task<IActionResult> EditEmployee([FromRoute] long employeeId, [FromBody] EmployeeEditDTO editedEmployeeDTO)
        {
            if (editedEmployeeDTO == null)
            {
                return BadRequest("Invalid data");
            }

            var employee = await _context.Employees.FindAsync(employeeId);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            _mapper.Map(editedEmployeeDTO, employee);

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Data received successfully", Data = editedEmployeeDTO });
        }
    }
}
