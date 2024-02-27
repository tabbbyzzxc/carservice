using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class Specialization
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Hours { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
