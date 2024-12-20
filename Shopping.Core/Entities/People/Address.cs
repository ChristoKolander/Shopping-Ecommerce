using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Entities.People
{
    public class Address
    {
        public Address() { }

        public string Street { get; set; } 
        public string City { get; set; } 
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public bool IsPrimary {  get; set; }

    }
}
