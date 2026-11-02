using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using WebTechLab.Data;
using WebTechLab.Models;

namespace WebTechLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public CategoriesController(AppDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult GetCategory()
        {
            return Ok(_context.Categories.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            string cacheKey = "categories_list";
            if (!_memoryCache.TryGetValue(cacheKey, out var categories))
            {
                categories = _context.Categories.ToList();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                _memoryCache.Set(cacheKey, categories, cacheOptions);
            }

            return Ok(categories);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.Id) return BadRequest();

            _context.Categories.Update(category);
            _context.SaveChanges();
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
    }
}