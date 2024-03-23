namespace MyShoeStudio.Data.Models
{
    public class Product_ShoppingList
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ShoppingListId { get; set; }
        public int Amount { get; set; }

        // Navigation properties
        public virtual Product? Product { get; set; }
        public virtual ShoppingList? ShoppingList { get; set; }
    }
}