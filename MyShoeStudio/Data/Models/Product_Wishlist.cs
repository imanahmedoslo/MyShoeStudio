namespace MyShoeStudio.Data.Models
{
    public class Product_Wishlist
    {
                public int Id { get; set; }
        public int ProductId { get; set; }
        public int WishlistId { get; set; }

        // Navigation properties
        public virtual Product Product { get; set; } = new Product();
        public virtual Wishlist Wishlist { get; set; } = new Wishlist();
    }
}