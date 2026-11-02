using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTechLab.Data;
using WebTechLab.Models;
using Microsoft.AspNetCore.SignalR;
using WebTechLab.Hubs;

namespace WebTechLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ProductsController(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/products (READ)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 10)
        {
            // Максимальний ліміт для захисту бази від перевантаження
            if (limit > 100) limit = 100;

            var products = await _context.Products
                                         .Include(p => p.Category)
                                         .Skip(skip)
                                         .Take(limit)
                                         .ToListAsync();

            return Ok(products);
        }

        // GET: api/products/5 (READ)
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Category)
                                                 .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(); 
            }
            return product; 
        }

        // POST: api/products (CREATE)
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            // Надсилання повідомлення всім підключеним клієнтам через SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Новинка у каталозі: {product.Name}!");

            // 2. Інтеграція Telegram API (B2)
            try
            {
                string botToken = "8986964757:AAFZLzPnXqpFlc0CdsGor-kqrqg_12IvhYM";
                string chatId = "766231543";
                string message = $"Новий товар додано на сайт!\nНазва: {product.Name}\nЦіна: {product.Price} грн.";

                string telegramUrl = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(message)}";

                using var httpClient = new HttpClient();
                await httpClient.GetAsync(telegramUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка відправки в Telegram: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/5 (UPDATE)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest(); 
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == id))
                {
                    return NotFound(); 
                }
                throw;
            }

            return NoContent(); 
        }

        // DELETE: api/products/5 (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(); 
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        [HttpGet("search")]
        public IActionResult SearchProducts([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Length < 3)
            {
                return Ok(new List<Product>());
            }

            var results = _context.Products
                .Where(p => p.Name.ToLower().Contains(q.ToLower()))
                .Take(10)
                .ToList();

            return Ok(results);
        } 
    }
}
