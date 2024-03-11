namespace MyShoeStudio.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }=string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }=false;
        public string UserName { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}

