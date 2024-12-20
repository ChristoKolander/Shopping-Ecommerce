using Microsoft.EntityFrameworkCore;
using Shopping.Core.Entities;
using Shopping.Core.Entities.People;
using Shopping.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Infrastructure.Data.Repositories
{
    
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        #region Fields and CTOR

        private readonly ProductContext productContext;

        public CustomerRepository(ProductContext ProductContext)
            : base(ProductContext)
        {
            productContext = ProductContext;
        }

        #endregion

        # region GetCustomers

        public async Task<Customer> GetCustomer(int id)
        {
            return await FindByCondition(c => c.Id == id)
                          .FirstOrDefaultAsync(c => c.Id == id);

        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await FindAll().ToListAsync();
        }


        // Make it a list in case of lastName are alike
        public async Task<Customer> GetCustomerByLastName(string lastName)
        {
            return await FindByCondition(c => c.LastName == lastName)
                          .FirstOrDefaultAsync(c => c.LastName == lastName);

        }
      
        public async Task<IEnumerable<Customer>> GetCustomersByCountry(string country)
        {

            return await FindAll()
                .Where(c => c.Details.Addresses.Any(a => a.Country == country))
                .ToListAsync();


        }

        public async Task<IEnumerable<Customer>> GetCustomersByVisits(int visits)
        {

            return await FindAll()
                .Where(c => c.Visits.Count > visits)
                .ToListAsync();


        }


        public async Task<IEnumerable<Customer>> GetCustomersByVisitByYear(int from, int to)
        {
           
            var customersInRange = await productContext.Set<Customer>()
                .Where(c => c.Visits.Any(v => v.Year >= from && v.Year <= to))
                .ToListAsync();

            return customersInRange;


        }

        public async Task<IEnumerable<Customer>> GetCustomersByVisitByYear2()
        {

            var specialDays = new List<DateOnly>
            {
                new(2015, 4, 1), new(2012,10,10), new(2019, 1, 4),
                new(2022,7,7), new(2013,12,12), new(1999,5,22)
            };

           
            var customerwhojoinedonspecialdays = await productContext.Set<Customer>()
                .Where(c => specialDays.Contains(c.MemberSince))
                .ToListAsync();

            return customerwhojoinedonspecialdays;



        }

        #endregion

    }
}