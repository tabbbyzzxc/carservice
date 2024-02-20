using BusinessEntities;

namespace WebApi.DTOs.Employees
{
    public class EmployeeDTO
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public List<Specialization> Specializations { get; set; }
    }
}
