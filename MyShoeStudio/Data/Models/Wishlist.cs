using Microsoft.EntityFrameworkCore;

namespace MyShoeStudio.Data.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalPrice { get; set; }
        public DateTime DateCreated { get; set; }

        
        public string ListName { get; set; }=string.Empty;

        // Navigation properties
        public virtual User User { get; set; }=new User();
        public virtual ICollection<Product_Wishlist> ProductPaths { get; set; }=new List<Product_Wishlist>();
    }
}