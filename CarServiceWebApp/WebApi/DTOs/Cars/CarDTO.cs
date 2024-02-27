using BusinessEntities;

namespace WebApi.DTOs.Cars
{
    public class CarDTO
    {
        public long Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string VIN { get; set; }

        public long ClientId { get; set; }

        public bool IsActive { get; set; }
    }
}