using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Entities.People
{
    public class Customer : Person
    {
        public Customer(){ }
            
        public string Title { get; set; }
        public DateOnly MemberSince { get; set; }
        public List<DateOnly> Visits { get; set; }
        public CustomerDetails Details { get; set; }     
       
    }
}
