using Microsoft.AspNetCore.Identity;
namespace MyShoeStudio.Data.Models
{
    public class User:IdentityUser
    {
       
       
       
        public bool IsAdmin { get; set; }=false;
       

        // Navigation properties
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}

