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

        [HttpPost]
        public IActionResult CreateReview([FromBody] Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return Ok(review);
        }
    }
}
