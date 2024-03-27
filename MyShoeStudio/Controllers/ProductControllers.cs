using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShoeStudio.Data;
using MyShoeStudio.Data.Models;
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
       // [Authorize(Roles = "Admin")]
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProduct product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Product newProduct = new Product()
            {
                Brand = product.Brand,
                Categories = product.Categories,
                CountryOfOrigin = product.CountryOfOrigin,
                Images = product.Images,
                Price = product.Price,
                Title = product.Title
            };
          await  _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            Size? size = new Size() { SizeValue = product.Size };
            bool sizeExists = await _context.Sizes.AnyAsync(x => x.SizeValue == product.Size);
            if (!sizeExists)
            {
              await  _context.Sizes.AddAsync(size);
            }
            else
            {
                size = await _context.Sizes.FirstOrDefaultAsync(x => x.SizeValue == product.Size);
            }
            await _context.SaveChangesAsync();
            ProductInventory productInventory = new ProductInventory() { Quantity = product.Amount, ProductId = newProduct.Id, SizeId = size.Id };
            await _context.ProductInventory.AddAsync(productInventory);
            await _context.SaveChangesAsync();
            return Ok(newProduct);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] CreateProduct product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Product newProduct = new Product() { Brand = product.Brand, Categories = product.Categories,
                CountryOfOrigin = product.CountryOfOrigin, Images = product.Images, Price = product.Price,
                Title = product.Title};
            _context.Products.Update(newProduct);
            await _context.SaveChangesAsync();
            Size? size = new Size() { SizeValue= product.Size };
            bool sizeExists = await _context.Sizes.AnyAsync(x => x.SizeValue == product.Size);
            if (!sizeExists)
            {
                _context.Sizes.Add(size);
                await _context.SaveChangesAsync();
            }else
            {
                size = await _context.Sizes.FirstOrDefaultAsync(x => x.SizeValue == product.Size);
            }
            ProductInventory productInventory = new ProductInventory() { Quantity = product.Amount, ProductId = newProduct.Id, SizeId = size.Id };
            bool productInventoryExists = await _context.ProductInventory.AnyAsync(x => x.ProductId == newProduct.Id && x.SizeId == size.Id);
            if (productInventoryExists)
            {
                ProductInventory? existingProductInventory = await _context.ProductInventory.FirstOrDefaultAsync(x => x.ProductId == newProduct.Id && x.SizeId == size.Id);
                existingProductInventory.Quantity += product.Amount;
                _context.ProductInventory.Update(existingProductInventory);
            }
            else
            {
                await _context.ProductInventory.AddAsync(productInventory);
            }

            return Ok(new { message = "Product updated successfully" });
        }

        [HttpGet("getProduct/{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _context.Products.Include(x=>x.ProductInventories).ThenInclude(pI=>pI.Size).FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("getAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.Include(x => x.ProductInventories).ThenInclude(pI => pI.Size).ToListAsync();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [Authorize(Roles="Admin")]
        [HttpDelete("deleteProduct/{productId}")]
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

public class CreateProduct
{
        public string? Title { get; set; }
    public List<string>? Images { get; set; }
    public int Price { get; set; }
    public string? Brand { get; set; }
    public string? CountryOfOrigin { get; set; }
    public ICollection<eCategory>? Categories { get; set; }
    public int Amount { get; set; }
    public eSize Size { get; set; }
}
