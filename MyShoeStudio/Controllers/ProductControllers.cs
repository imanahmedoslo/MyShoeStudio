using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShoeStudio.Data;
using MyShoeStudio.Data.Models;
using System.Security.Claims;
namespace MyShoeStudio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductControllers:ControllerBase
    {
        private MyShoeStudioDbContext _context;
        public ProductControllers(MyShoeStudioDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product added successfully" });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpGet("getProduct")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("getAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [Authorize(Roles="Admin")]
        [HttpDelete("deleteProduct")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product deleted successfully" });
        }
    }
}
