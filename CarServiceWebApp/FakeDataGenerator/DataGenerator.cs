using Bogus;
using BusinessEntities;

namespace FakeDataGenerator
{
    public class DataGenerator
    {
        public List<Client> GenerateRandomClients(int count)
        {
            var cars = new Faker<Car>()
                .RuleFor(c => c.Make, f => f.Vehicle.Manufacturer())
                .RuleFor(c => c.Model, f => f.Vehicle.Model())
                .RuleFor(c => c.VIN, f => f.Vehicle.Vin())
                .RuleFor(c => c.Year, f => f.Random.Number(1980, 2024));

            var clients = new Faker<Client>()
                //Basic rules using built-in generators
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Address, (f, u) => f.Address.StreetAddress())
                .RuleFor(u => u.City, (f, u) => f.Address.City())
                .RuleFor(u => u.PhoneNumber, (f, u) => f.Phone.PhoneNumberFormat())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
                .RuleFor(u => u.Cars, f => cars.Generate(f.Random.Number(1, 2)).ToList());

            var clientList = clients.Generate(count);

            return clientList;
        }

        public List<Material> GenerateRandomMaterials(int count)
        {
            var materials = new Faker<Material>()
                .RuleFor(u => u.Name, (f, u) => f.Commerce.ProductName())
                .RuleFor(u => u.Quantity, (f, u) => f.Random.Int(1, 100))
                .RuleFor(u => u.Price, (f, u) => f.Random.Decimal(1, 10000));

            var materialList = materials.Generate(count);

            return materialList;
        }

        public List<Employee> GenerateRandomEmployees(int count)
        {
            List<string> Specializations = new List<string>
            {
                "Mechanical Repair",
                "Electrical Systems",
                "Diagnostic Technician",
                "Bodywork",
                "Painting",
                "Wheel Alignment",
                "Air Conditioning",
                "Transmission Repair",
                "Brake Systems",
                "Suspension Systems",
                "Engine Rebuilding"
            };

            var shuffledSpecializations = Specializations.OrderBy(x => Guid.NewGuid()).ToList();

            var specs = new Faker<Specialization>()
                .RuleFor(s => s.Name, f => shuffledSpecializations[f.IndexGlobal % shuffledSpecializations.Count])
                .RuleFor(s => s.Hours, (f, e) => f.Random.Int(1, 24))
                .RuleFor(s => s.Price, (f, e) => f.Random.Decimal(200, 5000));

            var employees = new Faker<Employee>()
                //Basic rules using built-in generators
                .RuleFor(e => e.FirstName, (f, e) => f.Name.FirstName())
                .RuleFor(e => e.LastName, (f, e) => f.Name.LastName())
                .RuleFor(e => e.Address, (f, e) => f.Address.StreetAddress())
                .RuleFor(e => e.City, (f, e) => f.Address.City())
                .RuleFor(e => e.PhoneNumber, (f, e) => f.Phone.PhoneNumberFormat())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email())
                .RuleFor(e => e.Specializations, f => specs.Generate(f.Random.Number(1, 2)).ToList());

            var employeeList = employees.Generate(count);

            return employeeList;
        }

        public ServiceOptions GenerateRandomOptions()
        {

            ServiceOptions options = new()
            {
                Main = 10,
                Contacts = "{\"City\":\"New York\",\"Country\":\"USA\",\"State\":\"NY\",\"ZipCode\":\"10001\",\"Street\":\"123 Main St\", \"PhoneNumber\":\"+380931359244\", \"Email\":\"bambambam@gmail.com\"}",
                WeekDays = @"
{
  ""Monday"": {
    ""ShiftStartHour"": 9,
    ""ShiftStartMinute"": 0,
    ""ShiftEndHour"": 17,
    ""ShiftEndMinute"": 0
  },
  ""Tuesday"": {
    ""ShiftStartHour"": 9,  
    ""ShiftStartMinute"": 0,
    ""ShiftEndHour"": 17,
    ""ShiftEndMinute"": 0
  },
  ""Wednesday"": {
    ""ShiftStartHour"": 9,
    ""ShiftStartMinute"": 0,
    ""ShiftEndHour"": 17,
    ""ShiftEndMinute"": 0
  },
  ""Thursday"": {
    ""ShiftStartHour"": 9,
    ""ShiftStartMinute"": 0,
    ""ShiftEndHour"": 17,
    ""ShiftEndMinute"": 0
  },
  ""Friday"": {
    ""ShiftStartHour"": 9,
    ""ShiftStartMinute"": 0,
    ""ShiftEndHour"": 17,
    ""ShiftEndMinute"": 0
  },
  ""Saturday"": {
    ""ShiftStartHour"": 10,
    ""ShiftStartMinute"": 0,
    ""ShiftEndHour"": 16,
    ""ShiftEndMinute"": 0
  },
  ""Sunday"": {
    ""ShiftStartHour"": 10,
    ""ShiftStartMinute"": 0,
    ""ShiftEndHour"": 16,
    ""ShiftEndMinute"": 0
  }
}"
            };

            return options;
        }

    }
}