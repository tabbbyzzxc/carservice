using WebApi.DTOs.Clients;

namespace WebApi.DTOs.Employees
{
    public class EmployeeListDTO
    {
        public List<EmployeeDTO> Employees { get; set; }

        public int Total { get; set; }
    }
}
