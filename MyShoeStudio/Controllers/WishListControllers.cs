using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShoeStudio.Data;
using MyShoeStudio.Data.Models;
using System.Numerics;

namespace MyShoeStudio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListControllers:ControllerBase
    {
        private MyShoeStudioDbContext _context;
        public WishListControllers(MyShoeStudioDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "User")]
        [HttpPost("CreateWishList")]
        public async Task<IActionResult> CreateWishList([FromBody] CreateWishList wishList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Wishlist newWishList = new Wishlist()
            {
                TotalPrice = wishList.TotalPrice,
                UserId = wishList.UserId
            };
            await _context.Wishlists.AddAsync(newWishList);
            return Ok(new { message = "Product added to wish list successfully" });
        }
        [Authorize(Roles = "User, Admin")]
        [HttpDelete("DeleteWishList/{id}")]
        public async Task<IActionResult> DeleteWishList(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Wishlist? wishListToDelete = await _context.Wishlists.FirstOrDefaultAsync(x => x.UserId ==Id);
            if (wishListToDelete == null)
            {
                return NotFound(new { message = "Wish list not found" });
            }
            _context.Wishlists.Remove(wishListToDelete);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Wish list deleted successfully" });
        }
        [Authorize(Roles="User,Admin")]
        [HttpGet("GetWishListByDate")]
        public async Task<IActionResult> GetWishList([FromBody] WishlistName name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Wishlist? wishList = await _context.Wishlists.Include(w=>w.ProductPaths).ThenInclude(p=>p.Product).FirstOrDefaultAsync(w => w.ListName == name.ListName);
            if (wishList == null)
            {
                return NotFound(new { message = "Wish list not found" });
            }
            return Ok(wishList);
        }
        [Authorize(Roles="User,Admin")]
        [HttpPut("AddProductToWishList/{id}")]
        public async Task<IActionResult> AddProductToWishList(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Wishlist? wishListToAddProduct = await _context.Wishlists.FirstOrDefaultAsync(x => x.UserId == id);
            if (wishListToAddProduct == null)
            {
                return NotFound(new { message = "Wish list not found" });
            }
            Product_Wishlist product_Wishlist = new Product_Wishlist()
            {
                ProductId = id,
                WishlistId = wishListToAddProduct.Id
            };
            await _context.Product_Wishlists.AddAsync(product_Wishlist);
            return Ok(new { message = "Product added to wish list successfully" });
        }
        [Authorize(Roles="User,Admin")]
        [HttpDelete("RemoveProductFromWishList")]
        public async Task<IActionResult> RemoveProductFromWishList(DeleteWishList form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Wishlist? wishListToRemoveProduct = await _context.Wishlists.FirstOrDefaultAsync(x => x.UserId == form.wishListId);
            if (wishListToRemoveProduct == null)
            {
                return NotFound(new { message = "Wish list not found" });
            }
            Product_Wishlist? product_Wishlist = await _context.Product_Wishlists.FirstOrDefaultAsync(x => x.ProductId == form.ProductId && x.WishlistId == wishListToRemoveProduct.Id);
            if (product_Wishlist == null)
            {
                return NotFound(new { message = "Product not found in wish list" });
            }
            _context.Product_Wishlists.Remove(product_Wishlist);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product removed from wish list successfully" });
        }
        [Authorize(Roles="User,Admin")]
        [HttpGet("GetWishListsByUserId/{userId}")]
        public async Task<IActionResult> GetWishListsByUserId(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Wishlist> wishLists = await _context.Users.Where(u=>u.Id==userId).SelectMany(u=>u.Wishlists).Include(w=>w.ProductPaths).ThenInclude(p=>p.Product).ToListAsync();
            if (wishLists == null)
            {
                return NotFound(new { message = "Wish list not found" });
            }
            return Ok(wishLists);
        }


    }
}
public class CreateWishList
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int TotalPrice { get; set; }
    public string ListName { get; set; }=string.Empty;
}

public class WishlistName
{
    public string ListName { get; set; } = string.Empty;
}
public class DeleteWishList
{
    public int wishListId { get; set; }
    public int ProductId { get; set; }
}

