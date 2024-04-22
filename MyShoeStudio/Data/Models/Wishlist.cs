using Microsoft.EntityFrameworkCore;

namespace MyShoeStudio.Data.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TotalPrice { get; set; }
        public DateTime DateCreated { get; set; }

        
        public string ListName { get; set; }=string.Empty;

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<Product_Wishlist>? ProductPaths { get; set; }
    }
}