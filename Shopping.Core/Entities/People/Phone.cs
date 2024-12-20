using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Entities.People
{
    public class Phone
    {
        public Phone() { }

        public int CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPrimary { get; set; }
    }
}
