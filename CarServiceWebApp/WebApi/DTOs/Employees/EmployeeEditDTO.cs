﻿using BusinessEntities;

namespace WebApi.DTOs.Employees
{
    public class EmployeeEditDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

    }
}
