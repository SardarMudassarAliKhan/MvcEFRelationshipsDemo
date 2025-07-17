using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MvcEFRelationshipsDemo.Data;
using MvcEFRelationshipsDemo.Models;

namespace MvcEFRelationshipsDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public CustomersController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(int page = 1, int pageSize = 5)
        {
            var cacheKey = $"customers_page_{page}";

            if (!_cache.TryGetValue(cacheKey, out List<Customer> customers))
            {
                customers = await _context.Customers
                    .Include(c => c.Orders)
                    .AsNoTracking()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                _cache.Set(cacheKey, customers, TimeSpan.FromMinutes(2));
            }

            return Ok(customers);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomers), new { id = customer.CustomerId }, customer);
        }
    }
}
