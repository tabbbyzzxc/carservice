using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class Car
    {
        public long Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string VIN { get; set; }

        public long ClientId { get; set; }

        public Client Client { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
