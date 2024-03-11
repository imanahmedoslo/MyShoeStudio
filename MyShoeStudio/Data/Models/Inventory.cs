namespace MyShoeStudio.Data.Models
{
    public class Inventory
    {

        public int Id { get; set; }
        public string ProductInfo { get; set; }
        public int Amount { get; set; }

        // Navigation property
        public virtual Product Product { get; set; }
    }
}