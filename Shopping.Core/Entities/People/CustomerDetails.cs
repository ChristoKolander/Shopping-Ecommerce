using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Entities.People
{
    public class CustomerDetails
    {
        public CustomerDetails() { }

        public string Region {  get; set; }
        public List<string> Notes { get; set; } 
        public List<Phone> PhoneNumbers { get; set; } = new List<Phone>();
        public List<Address> Addresses { get; set; } = new List<Address>();


    }
}
