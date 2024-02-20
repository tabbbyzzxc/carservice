namespace WebApi.DTOs.Cars
{
    public class CarCreateDTO
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string VIN { get; set; }

        public long ClientId { get; set; }
    }
}
