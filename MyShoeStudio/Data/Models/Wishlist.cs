namespace MyShoeStudio.Data.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalPrice { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<Product_Wishlist> ProductPaths { get; set; }
    }
}