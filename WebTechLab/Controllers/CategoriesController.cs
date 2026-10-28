using Microsoft.AspNetCore.Mvc;
using WebTechLab.Data;
using WebTechLab.Models;
using System.Linq;

namespace WebTechLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCategory()
        {
            return Ok(_context.Categories.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

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