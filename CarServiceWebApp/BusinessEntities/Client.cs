namespace BusinessEntities
{
    public class Client
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public List<Car> Cars { get; set; }

        public bool IsActive { get; set; } = true;
    }
}