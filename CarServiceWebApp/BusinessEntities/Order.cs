using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class Order
    {
        public long Id { get; set; }

        public List<Material> MaterialsUsed { get; set; }

        public List<Employee> EmployeesId { get; set; }

        public long ClientId { get; set; }
    }
}
