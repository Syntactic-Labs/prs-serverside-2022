using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prs_serverside_2022.Models;
using System.Text.Json;

namespace prs_serverside_2022.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly JsonSerializerOptions jOptions = new() { WriteIndented= true };
        private readonly AppDbContext _context;
        private readonly ILoggerManager _logger;

        public ProductsController(AppDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {

            var products = await _context.Products.Include(p => p.Vendor).ToListAsync();

            if (products is null){
                _logger.LogError("Could not find: <List<Product>>");
                return NotFound("Fail to connect to DataBase or No data in table");
            };
            return products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            _logger.LogInfo($"Requested Id: {id}");
            var product = await _context.Products
                                                .Include(p => p.Vendor)
                                                .SingleOrDefaultAsync(p => p.Id == id);
            if (product == null){
                _logger.LogError($"Could not find productId: {id}");
                return NotFound();
            }

            _logger.LogInfo($"Response from Id: {JsonSerializer.Serialize(product, jOptions)}");
            return product;
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))  return NotFound();
                else throw;
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
