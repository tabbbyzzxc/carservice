using AutoMapper;
using Bogus;
using BusinessEntities;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text.Json;
using System.Transactions;
using WebApi.DTOs.ServiceOptions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private CarsDbContext _context;
        private IMapper _mapper;

        public OptionsController(IMapper mapper, CarsDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        private ServiceOptionDTO DeserializeOptions()
        {
            var optionsJSON = _context.ServiceOptions.AsNoTracking().First(o => o.Id == 1);

            ServiceOptionDTO options = new()
            {
                Contacts = JsonSerializer.Deserialize<ContactsDTO>(optionsJSON.Contacts),
                Main = new()
                {
                    CarCapacity = optionsJSON.Main
                },
                Weekdays = JsonSerializer.Deserialize<WeekdaysDTO>(optionsJSON.WeekDays)
            };

            return options;
        }

        [HttpGet("get-options")]
        public async Task<ActionResult> GetOptions()
        {
            var options = DeserializeOptions();

            return Ok(new { Message = "Options recieved", Data = options });
        }

        [HttpPut("edit-options")]
        public async Task<ActionResult> EditOptions([FromBody] ServiceOptionDTO optionsDTO)
        {


            if (optionsDTO == null)
            {
                return BadRequest("Invalid data");
            }
            ServiceOptions options = _context.ServiceOptions.First(o => o.Id == 1);

            options.Main = optionsDTO.Main.CarCapacity;
            options.Contacts = JsonSerializer.Serialize<ContactsDTO>(optionsDTO.Contacts);
            options.WeekDays = JsonSerializer.Serialize<WeekdaysDTO>(optionsDTO.Weekdays);

            _context.Update(options);
            await _context.SaveChangesAsync();



            return Ok(new { Message = "Options edited" });



        }
    }
}
