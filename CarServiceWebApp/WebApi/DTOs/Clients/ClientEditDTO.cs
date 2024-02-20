using BusinessEntities;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Clients
{
    public class ClientEditDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
