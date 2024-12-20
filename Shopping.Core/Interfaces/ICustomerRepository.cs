using Microsoft.Extensions.Primitives;
using Shopping.Core.Entities;
using Shopping.Core.Entities.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Interfaces
{
    public interface ICustomerRepository: IRepositoryBase<Customer>
    {
        Task<Customer> GetCustomer(int id);

        Task<IEnumerable<Customer>> GetCustomers();

        //Make it a list in case of lastName are alike
        Task<Customer> GetCustomerByLastName(string lastName);

        Task<IEnumerable<Customer>> GetCustomersByCountry(string Country);

        Task<IEnumerable<Customer>> GetCustomersByVisits(int visits);

       

        Task<IEnumerable<Customer>> GetCustomersByVisitByYear(int from, int to);

        Task<IEnumerable<Customer>> GetCustomersByVisitByYear2();


    }
}
