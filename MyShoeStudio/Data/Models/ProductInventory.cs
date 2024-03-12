namespace MyShoeStudio.Data.Models
{
    public class ProductInventory
    {
        
            public int Id { get; set; }
            public int Quantity { get; set; }

            // Foreign keys to Product and Size
            public int ProductId { get; set; }
            public virtual Product Product { get; set; }

            public int SizeId { get; set; }
            public virtual Size Size { get; set; }
        
    }
}