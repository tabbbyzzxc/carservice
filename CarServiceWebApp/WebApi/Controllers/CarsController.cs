using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessEntities;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApi.DTOs.Cars;
using WebApi.DTOs.Clients;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private CarsDbContext _context;
        private IMapper _mapper;

        public CarsController(CarsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("get-car")]
        public async Task<CarDTO> GetCar([FromQuery] long id)
        {
            var query = _context.Cars.AsNoTracking();
            var car = query
                .Where(c => c.IsActive == true)
                .FirstOrDefault(c => c.Id == id);

            var carDTO = _mapper.Map<CarDTO>(car);

            return carDTO;
        }

        [HttpPost("post-car")]
        public async Task<CarListDTO> PostCar(CarCreateDTO newCarDTO)
        {
            Car newCar = _mapper.Map<Car>(newCarDTO);
            await _context.Cars.AddAsync(newCar);
            await _context.SaveChangesAsync();

            var cars = await _context.Cars.AsNoTracking()
                .Where(c => c.ClientId == newCar.ClientId && c.IsActive == true)
                .ToListAsync();

            var carsDTO = _mapper.Map<List<CarDTO>>(cars);
            return new CarListDTO { Cars = carsDTO };
        }

        [HttpPut("edit-car/{carId:long}")]
        public async Task<IActionResult> EditCar([FromRoute] long carId, [FromBody] CarEditDTO editedCarDTO)
        {
            if (editedCarDTO == null)
            {
                return BadRequest("Invalid data");
            }

            var car = await _context.Cars.FindAsync(carId);

            if (car == null)
            {
                return NotFound("Car not found");
            }

            _mapper.Map(editedCarDTO, car);

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();

            var clientCars = await _context.Cars
                .AsNoTracking()
                .Where(c => c.ClientId == car.ClientId && c.IsActive == true)
                .ToListAsync();

            return Ok(new { Message = "Data received successfully", Data = clientCars });
        }

        [HttpDelete("delete-car/{carId:long}")]
        public async Task<IActionResult> DeleteCar([FromRoute] long carId)
        {
            if (carId == null)
            {
                return BadRequest("Invalid data");
            }

            var car = await _context.Cars.FindAsync(carId);

            if (car == null)
            {
                return NotFound("Car not found");
            }

            car.IsActive = false;

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();

            var cars = await _context.Cars.AsNoTracking()
                .Where(c => c.ClientId == car.ClientId && c.IsActive == true)
                .ProjectTo<CarDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();


            return Ok(new { Message = "Car deleted successfully", Data = cars });
        }
    }
}
