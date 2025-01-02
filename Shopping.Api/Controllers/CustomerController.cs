using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Core.Entities.People;
using Shopping.Core.Interfaces;
using Shopping.Infrastructure.Data.Repositories;
using Shopping.Shared.Dtos.CRUDs;

namespace Shopping.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        #region Fields and CTOR

        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper, ILoggerManager logger)
        {

            this.customerRepository = customerRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        #endregion


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await customerRepository.GetCustomer(id);
            return Ok(customer);
        }

        [HttpGet("ByLastName/{lastName}")]
        public async Task<ActionResult<Customer>> GetCustomerByLastName(string lastName)
        {
            var customer = await customerRepository.GetCustomerByLastName(lastName);
            return Ok(customer);
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomers()
        {
            var customers = await customerRepository.GetCustomers();
            return Ok(customers);
        }

        [HttpGet("ByCountry/{country}")]
        public async Task<ActionResult<List<Customer>>> GetCustomersByCountry(string country)
        {
            var customers = await customerRepository.GetCustomersByCountry(country);
            return Ok(customers);
        }

        [HttpGet("ByVisits/{visits}")]
        public async Task<ActionResult<List<Customer>>> GetCustomersByVisits(int visits)
        {
            var customers = await customerRepository.GetCustomersByVisits(visits);
            return Ok(customers);
        }

        [HttpGet("ByVisitByYear")]
        public async Task<ActionResult<List<Customer>>> GetCustomersByVisitByYear(int from, int to)
        {
            var customers = await customerRepository.GetCustomersByVisitByYear(from, to);
            return Ok(customers);
        }

        [HttpGet("ByVisitByYear2")]
        public async Task<ActionResult<List<Customer>>> GetCustomersByVisitByYear2()
        {
            var customers = await customerRepository.GetCustomersByVisitByYear2();
            return Ok(customers);
        }     
    }
}