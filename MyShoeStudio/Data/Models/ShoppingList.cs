namespace MyShoeStudio.Data.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public int TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public bool IsPurchased { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<Product_ShoppingList> ProductPaths { get; set; }
    }
}