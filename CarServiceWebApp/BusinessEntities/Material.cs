using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class Material
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
