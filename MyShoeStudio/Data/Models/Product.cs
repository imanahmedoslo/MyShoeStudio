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
        public ICollection <eCategory>? Categories { get; set; }


        public virtual ICollection<ProductInventory>? ProductInventories { get; set; }


        public virtual ICollection<Product_ShoppingList>? ShoppingLists { get; set; }
        public virtual ICollection<Product_Wishlist>? Wishlists { get; set; }
    }
}