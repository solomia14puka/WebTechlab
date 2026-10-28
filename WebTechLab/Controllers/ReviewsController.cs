using Microsoft.AspNetCore.Mvc;
using WebTechLab.Data;
using WebTechLab.Models; 

namespace WebTechLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetReviews()
        {
            return Ok(_context.Reviews.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetReview(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, [FromBody] Review review)
        {
            if (id != review.Id) return BadRequest();

            _context.Reviews.Update(review);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review == null) return NotFound();

            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
