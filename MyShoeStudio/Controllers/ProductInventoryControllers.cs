using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShoeStudio.Data;
using MyShoeStudio.Data.Models;

namespace MyShoeStudio.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductInventoryControllers:ControllerBase
    {
        private MyShoeStudioDbContext _context;
        public ProductInventoryControllers(MyShoeStudioDbContext context)
        {
            _context = context;
        }
        [Authorize (Roles = "Admin")]
        [HttpPost("addProductToInventory")]
        public async Task<IActionResult> AddProductToInventory([FromBody] InventoryForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ProductInventory productInventory = new ProductInventory() { Quantity = form.Amount, ProductId = form.ProductId, SizeId = form.SizeId };
            await _context.ProductInventory.AddAsync(productInventory);
            return Ok(new { message = "Product added to inventory successfully" });
        }
        [Authorize (Roles = "Admin")]
        [HttpPut("updateProductAmount")]
        public async Task<IActionResult> UpdateProductAmount(UpdateAmount amountForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ProductInventory? productInventory = _context.ProductInventory.FirstOrDefault(x => x.Id == amountForm.Id);
            if (productInventory == null)
            {
                return NotFound(new { message = "Product inventory not found" });
            }
            productInventory.Quantity = amountForm.Amount;
            _context.ProductInventory.Update(productInventory);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product inventory updated successfully" });
        }
        [Authorize (Roles = "Admin")]
        [HttpDelete("deleteProductFromInventory/{id}")]
        public async Task<IActionResult> DeleteProductFromInventory(int id)
        {
            ProductInventory? productInventory = _context.ProductInventory.FirstOrDefault(x => x.Id == id);
            if (productInventory == null)
            {
                return NotFound(new { message = "Product inventory not found" });
            }
            _context.ProductInventory.Remove(productInventory);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product inventory deleted successfully" });
        }
       
        [HttpGet("getProductInventory")]
        public async Task<IActionResult> GetProductInventory()
        {
            List<ProductInvetoryObject> productInventory = await _context.ProductInventory.Select(i => new ProductInvetoryObject { Id=i.Id, Quantity=i.Quantity, ProductTitle=i.Product.Title,SizeValue=i.Size.SizeValue.ToString()??"null", ProductId=i.ProductId, SizeId=i.SizeId}).ToListAsync();
            return Ok(productInventory);
        }
    }
}

public class InventoryForm
{
    public int Amount { get; set; }
    public int ProductId { get; set; }
    public int SizeId { get; set; }
   
}
public class  UpdateAmount
{
  public  int Id { get; set; }
    public int Amount { get; set; }

}
public class ProductInvetoryObject
{ 
    public int Id { get; set; }
    public int Quantity { get; set; }
    public string? ProductTitle { get; set; }
    public string? SizeValue { get; set; }
    public int ProductId { get; set; }
    public int SizeId { get; set; }

}
