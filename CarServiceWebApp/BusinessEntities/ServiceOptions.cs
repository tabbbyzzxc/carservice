using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class ServiceOptions
    {
        public long Id { get; set; }

        public int Main { get; set; }

        public string Contacts { get; set; }

        public string WeekDays { get; set; }
    }
}
