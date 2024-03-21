namespace MyShoeStudio.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<string> Images { get; set; }=new List<string>();
        public int Price { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string CountryOfOrigin { get; set; } = string.Empty;
        public ICollection <eCategory> Categories { get; set; }=new List<eCategory>();


        public virtual ICollection<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();



        public virtual ICollection<Product_ShoppingList> ShoppingLists { get; set; } = new List<Product_ShoppingList>();
        public virtual ICollection<Product_Wishlist> Wishlists { get; set; }= new List<Product_Wishlist>();
    }
}