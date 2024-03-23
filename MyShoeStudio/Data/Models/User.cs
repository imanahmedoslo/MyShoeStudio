using Microsoft.AspNetCore.Identity;
namespace MyShoeStudio.Data.Models
{
    public class User:IdentityUser
    {
       
       
       
        public bool IsAdmin { get; set; }=false;
       

        // Navigation properties
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }=new List<ShoppingList>();
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public virtual PersonalInfo? PersonalInfo { get; set; } 
    }
}

