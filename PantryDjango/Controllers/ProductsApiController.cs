using Microsoft.AspNetCore.Mvc;
using PantryDjango.Data;

namespace PantryDjango.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly FoodDbContext _context;

        public ProductsController(FoodDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProductByBarcode(string barcode)
        {
            var product = _context.FoodItems
                .Where(f => f.Barcode == barcode) // Assuming you have a Barcode property
                .Select(f => new
                {
                    f.Name,
                    f.Description,
                    ExpirationDate = f.ExpirationDate.ToString("yyyy-MM-dd"),
                    f.Quantity,
                    f.Unit,
                    f.Category,
                    f.Location
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }

}
