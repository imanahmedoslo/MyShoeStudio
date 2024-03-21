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
        public async Task<IActionResult> AddShoppingList([FromBody] ShoppingList shoppingList)
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
            return Ok(new { message = "Shopping list added successfully" });
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPut("updateShoppingList")]
        public async Task<IActionResult> UpdateShoppingList([FromBody] ShoppingList shoppingList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ShoppingList newShoppingList = new ShoppingList()
            {
                TotalPrice = shoppingList.TotalPrice,
                Date = shoppingList.Date,
            };
            _context.ShoppingLists.Update(newShoppingList);
            return Ok(new { message = "Shopping list updated successfully" });
        }
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("deleteShoppingList")]
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
        [HttpGet("getShoppingList")]
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
        [HttpGet("getAllShoppingListByUserId")]
        public async Task<IActionResult> GetAllShoppingListByUserId(string userId)
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
        [HttpGet("getAllShoppingHistory")]
        public async Task<IActionResult> GetAllShoppingHistory(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<ShoppingList>? shoppingList = await _context.Users.Where(u=>u.Id==userId).SelectMany(u=>u.ShoppingLists).Include(s => s.ProductPaths).ThenInclude(p => p.Product).ToListAsync();
            if (shoppingList == null)
            {
                return NotFound(new { message = "Shopping list not found" });
            }
            return Ok(shoppingList);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPut("purchaseShoppingList")]
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
            return Ok(new { message = "Shopping list purchased successfully" });
        }

    }
}


