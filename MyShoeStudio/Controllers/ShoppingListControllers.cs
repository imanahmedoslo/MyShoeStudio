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
                UserId = shoppingList.UserId
            };
            await _context.ShoppingLists.AddAsync(newShoppingList);
            return Ok(new { message = "Shopping list added successfully" });
        }
    }
}
