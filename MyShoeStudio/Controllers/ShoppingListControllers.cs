using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShoeStudio.Data;
using MyShoeStudio.Data.Models;
namespace MyShoeStudio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListControllers:ControllerBase
    {
        private MyShoeStudioDbContext _context;
        public ShoppingListControllers(MyShoeStudioDbContext context)
        {
            _context = context;
        }
        [Authorize (Roles="Admin,User")]
        [HttpPost("addShoppingList")]
        public async Task<IActionResult> AddShoppingList([FromBody] CreateShoppingList shoppingList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ShoppingList newShoppingList = new ShoppingList()
            {
                TotalPrice = shoppingList.TotalPrice,
                Date = shoppingList.Date,
                UserId = shoppingList.UserId,
                IsPurchased = false,
            };
            await _context.ShoppingLists.AddAsync(newShoppingList);
            return Ok(newShoppingList);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPut("updateShoppingList")]
        public async Task<IActionResult> UpdateShoppingList([FromBody] CreateProduct_ShoppingList updates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Product_ShoppingList? productPath= _context.Product_ShoppingLists.FirstOrDefault(x => x.ProductId == updates.ProductId&& x.ShoppingListId==updates.ShoppingListId);
            if(productPath == null)
            {
                return NotFound(new { message = "Product path not found" });
            }
            if(updates.Amount == 0)
            {
                _context.Product_ShoppingLists.Remove(productPath);
              await  _context.SaveChangesAsync();
                return Ok(new { message = "Product path removed successfully" });
            }
            productPath.Amount = updates.Amount;
            _context.Product_ShoppingLists.Update(productPath);
            await _context.SaveChangesAsync();
            return Ok(productPath);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("deleteShoppingList/{id}")]
        public async Task<IActionResult> DeleteShoppingList(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           ShoppingList? shoppingList = await _context.ShoppingLists.FirstOrDefaultAsync(x => x.Id == id);
            if (shoppingList == null)
            {
                return NotFound(new { message = "Shopping list not found" });
            }
            _context.ShoppingLists.Remove(shoppingList);
            _context.SaveChanges();
            return Ok(new { message = "Shopping list deleted successfully" });
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("getShoppingList/{id}")]
        public async Task<IActionResult> GetShoppingList(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ShoppingList? shoppingList = await _context.ShoppingLists.Include(s=>s.ProductPaths).ThenInclude(p=>p.Product).FirstOrDefaultAsync(x => x.Id == id);
            if (shoppingList == null)
            {
                return NotFound(new { message = "Shopping list not found" });
            }
            return Ok(shoppingList);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("getAllShoppingListByUserId/{userId}")]
        public async Task<IActionResult> GetShoppingListByUserId(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ShoppingList? shoppingList = await _context.Users.Where(u=>u.Id==userId).SelectMany(u=>u.ShoppingLists).Include(s=>s.ProductPaths).ThenInclude(p=>p.Product).OrderByDescending(s=>s.Date).FirstOrDefaultAsync();
            if (shoppingList == null)
            {
                return NotFound(new { message = "Shopping list not found" });
            }
            
            
            return Ok(shoppingList);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("getAllShoppingHistory/{userId}")]
        public async Task<IActionResult> GetAllUserShoppingHistory(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<ShoppingList>? shoppingLists = await _context.Users.Where(u=>u.Id==userId).SelectMany(u=>u.ShoppingLists).Include(s => s.ProductPaths).ThenInclude(p => p.Product).ToListAsync();
            if (shoppingLists == null)
            {
                return NotFound(new { message = "Shopping list not found" });
            }
            return Ok(shoppingLists);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPut("purchaseShoppingList/{id}")]
        public async Task<IActionResult> PurchaseShoppingList(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ShoppingList? shoppingList = await _context.ShoppingLists.FirstOrDefaultAsync(x => x.Id == id);
            if (shoppingList == null)
            {
                return NotFound(new { message = "Shopping list not found" });
            }
            shoppingList.IsPurchased = true;
            _context.ShoppingLists.Update(shoppingList);
            await _context.SaveChangesAsync();
            return Ok(shoppingList);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPost("addProductToShoppingList")]
        public async Task<IActionResult> AddProductToShoppingList([FromBody] CreateProduct_ShoppingList productPath)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Product_ShoppingList newProductPath = new Product_ShoppingList()
            {
                ProductId = productPath.ProductId,
                ShoppingListId = productPath.ShoppingListId,
                Amount = productPath.Amount
            };
            await _context.Product_ShoppingLists.AddAsync(newProductPath);
            return Ok(newProductPath);
        }
    }
}
public class CreateShoppingList
{
    public int Id { get; set; }
    public int TotalPrice { get; set; }
    public DateTime Date { get; set; }
    public bool IsPurchased { get; set; }
    public string UserId { get; set; }

}
public class CreateProduct_ShoppingList
{
    public int ProductId { get; set; }
    public int ShoppingListId { get; set; }
    public int Amount { get; set; }
}




